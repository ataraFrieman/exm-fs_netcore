using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quze.DAL;
using Quze.DAL.Stores;
using Quze.Infrastruture.Extensions;
using Quze.Infrastruture.Security;
using Quze.Models.Entities;
using Quze.Organization.Web.Utilites;


namespace Quze.Organization.Web
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
            services.AddDbContext<QuzeContext>(options =>
                options.UseSqlServer(sqlConnectionString)
            );

            services.AddIdentityCore<User>();
            services.AddAutoMapper();
            services.AddQuzeJwtAuthentication();
            services.AddScoped<JwtTokenGenerator, JwtTokenGenerator>();

            services.AddTransient<IUserStore<User>, UserStore>();
            services.AddScoped<QueueStore, QueueStore>();
            services.AddScoped<OrganizationStore, OrganizationStore>();
            services.AddScoped<FellowStore, FellowStore>();
            services.AddScoped<ServiceTypeStore, ServiceTypeStore>();
            services.AddScoped<ServiceProviderStore, ServiceProviderStore>();
            services.AddScoped<BranchStore, BranchStore>();
            services.AddScoped<ServiceProvidersServiceTypeStore, ServiceProvidersServiceTypeStore>();
            services.AddScoped<AlertRuleStore, AlertRuleStore>();
            services.AddScoped<RequiredDocumentStore, RequiredDocumentStore>();
            services.AddScoped<RequiredTaskStore, RequiredTaskStore>();

            services.AddTransient<UserTasksStore, UserTasksStore>();
            //services.AddScoped<ClientSearch, ClientSearch>();

            services.AddHttpContextAccessor();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<ISMS, SlngSMS>();
            services.AddMemoryCache();

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddMvc(config =>
            {
                //only allow authenticated users
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();

                config.Filters.Add(new AuthorizeFilter(policy));
            }
            );
            services.AddMvc(); //.SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSignalR();

            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            loggerFactory.AddFile("Logs/Quze_Organuzation-{Date}.txt");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseAuthentication();
            app.UseMvcWithDefaultRoute();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

        }
    }
}
