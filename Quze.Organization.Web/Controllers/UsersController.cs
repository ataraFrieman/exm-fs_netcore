using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Models;
using Quze.Models.Entities;

using Quze.Organization.Web.ViewModels;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Quze.Organization.Web.Controllers
{

    [Route("api/[controller]")]
    public class UsersController : ControllerBase<User, Quze.Models.Models.ViewModels.UserVM, UserStore>
    {

        private readonly QuzeContext context;
        private readonly UserTasksStore userTasksStore;
        protected readonly QueueStore queueStore;

        public UsersController(
            QuzeContext ctx
            , QueueStore queueStore
            , IUserStore<User> store
            , IMapper mapper
           
            , UserTasksStore userTasksStore) : base(mapper, (UserStore)store)
        {
            context = ctx;
            //this.userTasksStore = userTasksStore;

            this.queueStore = queueStore;
            
            this.userTasksStore = userTasksStore;

        }


        [HttpGet("[action]/{organizationId}")]
        public List<User> GetUsersToOrganization(int organizationId)
        {
            if (organizationId == 0)
            {
                //return BadRequest("wrong request");
            }
            var Users = context.Users
                .Include(u => u.UserType)
                .Where(u => u.OrganizationId == organizationId &&
                u.UserTypeId == UserType.OrganizationUser)
                .OrderBy(u=>u.TitledFullName)
                .ToList();
            return Users;

        }



        [HttpPost("[action]")]
        public async Task<Response<UserTaskVM>> CreateUserTask([FromBody]Request<UserTaskVM> request)
        {
            var response = new Response<UserTaskVM>();
            //if (!TryValidateModel(request))
            //{

            //}

            if (request?.Entity == null)
            {
                response.AddError(655, "The parameter is not properly formatted or entity is null!");
                return response;
            }

            var userTask = mapper.Map<UserTask>(request.Entity);
            await userTasksStore.CreateAsync(userTask);

            var userTaskVM = mapper.Map<UserTaskVM>(userTask);

            response.Entity = userTaskVM;

            return response;
        }

        [HttpGet("[action]/{userId}")]
        public async Task<Response<UserTaskVM>> GetUserTasks(int userId)
        {
            var response = new Response<UserTaskVM>();

            if (userId == 0)
            {
                response.AddError(655, "Id must be not 0!");
                return response;
            }

            //var user = userStore.FindByIdAsync(userId.ToString());
            var userTasks = await userTasksStore.GetAll(userId);
            var userTasksVmList = mapper.Map<List<UserTaskVM>>(userTasks);
            response.Entities = userTasksVmList;

            return response;
        }

        [HttpPut("[action]")]
        public async Task<Response<UserTaskVM>> CloseExecutedUserTask([FromBody]Request<UserTaskVM> request)
        {
            var response = new Response<UserTaskVM>();
            //if (!TryValidateModel(request))
            //{

            //}

            if (request?.Entity == null)
            {
                response.AddError(655, "The parameter is not properly formatted or entity is null!");
                return response;
            }

            var userTask = mapper.Map<UserTask>(request.Entity);
            userTask.ExecutionDateTime = DateTime.Now;

            userTask = await userTasksStore.UpdateUserTask(userTask);

            var userTaskVM = mapper.Map<UserTaskVM>(userTask);

            response.Entity = userTaskVM;

            return response;
        }

    }
}
