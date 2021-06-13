    using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Security;
using Quze.Models.Entities;
using System.Reflection;

namespace Quze.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var sqlConnectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<Quze.DAL.QuzeContext>(options =>
               options.UseSqlServer(sqlConnectionString)
           );

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddAutoMapper(
                cfg =>
                {
                    cfg.ForAllMaps((obj, cnfg) => cnfg.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)));
                }
                );

            services.AddQuzeJwtAuthentication();
            services.AddScoped<JwtTokenGenerator, JwtTokenGenerator>();

            // ===== Add Identity ========
            //services.AddIdentity<IdentityUser, IdentityRole>();
            services.AddHttpContextAccessor();
            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddSingleton<IUserService, UserService>();

            services.AddScoped<QueueStore, QueueStore>();
            services.AddScoped<AlertStore, AlertStore>();
            services.AddScoped<AppointmentDocumentStore, AppointmentDocumentStore>();
            services.AddScoped<AppointmentStore, AppointmentStore>();
            services.AddScoped<AppointmentTaskStore, AppointmentTaskStore>();
            services.AddScoped<BranchStore, BranchStore>();
            services.AddScoped<CityStore, CityStore>();
            services.AddScoped<FellowStore, FellowStore>();
            services.AddScoped<MinimalKitRulesStore, MinimalKitRulesStore>();
            services.AddScoped<OrganizationStore, OrganizationStore>();
            services.AddScoped<RequiredDocumentStore, RequiredDocumentStore>();
            services.AddScoped<RequiredTaskStore, RequiredTaskStore>();
            services.AddScoped<ServiceProviderStore, ServiceProviderStore>();
            services.AddScoped<ServiceTypeStore, ServiceTypeStore>();
            services.AddScoped<ServiceProvidersServiceTypeStore, ServiceProvidersServiceTypeStore>();
            services.AddScoped<ServiceQueueStore, ServiceQueueStore>();
            services.AddScoped<StreetStore, StreetStore>();
            services.AddScoped<TimeTableExceptionStore, TimeTableExceptionStore>();
            services.AddScoped<TimeTableLineStore, TimeTableLineStore>();
            services.AddScoped<TimeTableStore, TimeTableStore>();
            services.AddScoped<TimeTableVacationStore, TimeTableVacationStore>();
            services.AddScoped<UserStore, UserStore>();
            services.AddScoped<UserTasksStore, UserTasksStore>();
            services.AddScoped<UserTypeStore, UserTypeStore>();
            services.AddScoped<ExpertyStore, ExpertyStore>();
            services.AddScoped<ServiceProvidersExpertiesStore, ServiceProvidersExpertiesStore>();
            services.AddScoped<AlertRuleStore, AlertRuleStore>();

            services.AddMvc(config =>
                {
                    //only allow authenticated users
                    var policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .Build();

                    config.Filters.Add(new AuthorizeFilter(policy));
                }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors("AllowAll");
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                //routes.MapRoute(
                //    name: "default",
                //    template: "{controller=Home}/{action=Index}");
            });
            //app.UseMvcWithDefaultRoute();

            Mapper.Initialize(cfg =>
            {
                cfg.ForAllMaps((obj, cnfg) => cnfg.ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null)));
            });
        }
    }
}