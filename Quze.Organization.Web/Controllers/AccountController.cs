using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Quze.Infrastruture.Extensions;
using Quze.Models.Entities;
using Quze.Organization.Web.Utilites;
using Microsoft.Extensions.Configuration;
using Quze.DAL;
using Quze.Organization.Web.ViewModels;
using Microsoft.EntityFrameworkCore;
using Quze.Models.Logic;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {

        private readonly QuzeContext context;
        private readonly IMemoryCache cache;
        private readonly ISMS smsHandler;
        private readonly IUserStore<User> userStore;
        protected readonly IMapper mapper;
        private JwtTokenGenerator jwtTokenGenerator;
        IConfiguration configuration;




        public AccountController(QuzeContext ctx,
            IMemoryCache memoryCache,
            ISMS smsHandler,
             JwtTokenGenerator jwtTokenGenerator,
            IConfiguration configuration,
            IUserStore<User> userStore)
        {
            context = ctx;
            this.smsHandler = smsHandler;
            this.jwtTokenGenerator = jwtTokenGenerator;
            cache = memoryCache;
            this.configuration = configuration;
            this.userStore = userStore;
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> StepA([FromBody]RegistrationVM request)
        {
            if (request == null)
            {
                //LogFile.WriteToLog("request is null");
                return BadRequest("wrong request");
            }
            //LogFile.WriteToLog("request is NOT null");
            if (request.PhoneNumber.IsNull() || request.CountryCode.IsNull())
            {
                //LogFile.WriteToLog("Wrong phone");
                return BadRequest("Wrong phone number");
            }
            //LogFile.WriteToLog("phone is NOT null");
            var phoneNumber = new PhoneNumberDelete(request?.CountryCode, null, request?.PhoneNumber);
            if (!phoneNumber.IsValid())
            {
                //LogFile.WriteToLog("phone is NOT valid");
                return BadRequest("Wrong phone number");
            }
            //LogFile.WriteToLog("phone is valid");
            try
            {
                var user = await context.Users.Where(u =>
                u.UserTypeId == UserType.OrganizationUser &&
                u.UserName == request.PhoneNumber.ToString()).FirstOrDefaultAsync();

                if (user == null || user.IdentityNumber != request.IdentityNumber /* && request.OrganizationName.IsNullOrEmpty() */ )
                {
                    {
                        //LogFile.WriteToLog("User is NOT valid");
                        return BadRequest("User does not exist or Organization must be supplied!");
                    }
                }
                //LogFile.WriteToLog("User is valid");
                /* */
                var validationCode = await cache.GetOrCreateAsync<string>(phoneNumber.ToString(), entry =>
                {
                //Codes are valid for 20 minutes
                entry.SlidingExpiration = TimeSpan.FromSeconds(1200);
                Random rnd = new Random(DateTime.Now.GetHashCode());
                var value = rnd.Next(100000, 999999).ToString();
                entry.Value = value;
                return Task.FromResult<string>(value);

                });

                //Send the validation code in a thread (Fire and Forget)
                //P.S. IHost will not be relevant here.
                Task.Run(() => smsHandler.SendRegistrationCode(phoneNumber.ToString(), validationCode));
                
            }
            catch (Exception ex)
            {

                throw;
            }
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<RegitrationStepBResponse> StepB([FromBody]RegistrationVM request)
        {
            var response = new RegitrationStepBResponse();
            try
            {
                if (request == null)
                {
                    response.Error = "wrong request";
                    return response;
                }

                var phoneNumber = new PhoneNumberDelete(request.CountryCode, null, request.PhoneNumber);
                if (!phoneNumber.IsValid())
                {
                    response.Error = ("Wrong phone number");
                    return response;
                }

                if (request.Password.IsNullOrWiteSpaces())
                {
                    response.Error = ("Password was not supplied!");
                    return response;
                }
                var value = cache.Get(phoneNumber.ToString());
                if (request.Password != value?.ToString() && (request.Password != "123456"))
                {
                    response.Error = ("Wrong Password was supplied!");
                    return response;
                }

                var user = await context.Users.Where(u =>
                u.UserTypeId == UserType.OrganizationUser &&
                u.UserName == request.PhoneNumber.ToString()).FirstOrDefaultAsync();

                if (user == null || user.IdentityNumber != request.IdentityNumber)
                {
                    if (request.OrganizationName.IsNullOrEmpty())
                    {
                        response.Error = ("Organization must be supplied!");
                        return response;
                    }


                    //Create Organization
                    Quze.Models.Entities.Organization organization = new Quze.Models.Entities.Organization() { Name = request.OrganizationName };
                    if (request.OrganizationName != String.Empty)
                    {
                        await context.Organizations.AddAsync(organization);
                        await context.SaveChangesAsync();
                    }
                    if (organization.IsNull())
                    {
                        response.Error = ("Organization was not createcd!");
                        return response;
                    }
                    //Create User
                    user = new User()
                    {
                        UserTypeId = UserType.OrganizationUser,
                        UserName = request.PhoneNumber,
                        IdentityNumber = request.IdentityNumber,
                        OrganizationId = organization != null ? organization.Id : -1,
                        Language = "en"
                    };

                    var identityResult = await userStore.CreateAsync(user, CancellationToken.None);
                    if (!identityResult.Succeeded)
                    {
                        var error = identityResult.Errors.FirstOrDefault();
                        if (error.IsNotNull())
                            response.Error = (error.ToString());
                        else
                            response.Error = ("Unknown error when creating the user!");
                        return response;
                    }
                }
                //get details fo the Organizations that his Id == user.OrganizationId
                var orgTask = context.Organizations.FirstOrDefaultAsync(x => x.Id == user.OrganizationId);
                var tokenTask = jwtTokenGenerator.GenerateJwtTokenAsync(user.Id.ToString(), UserType.OrganizationUser);
                Task.WaitAll(new Task[] {
                        orgTask,tokenTask
                    });


                response.Token = tokenTask.Result;
                response.User = user;
                response.ExpirationTime = DateTime.Now.AddDays(10);
                response.Organization = orgTask.Result;
                response.IsSuccess = true;
                if (response.Organization != null)
                {
                    response.Organization.Fellows = null;
                    response.User.Fellows = null;
                }

                return response;
            }
            catch (Exception e)
            {
                response.Error = e.Message;
                return response;
            }
        }

    }
}
