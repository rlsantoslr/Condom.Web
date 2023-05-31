using Condom.Domain.Models;
using Condom.Infra.App;
using Condom.Infra.Claims;
using Condom.Infra.Context;
using Condom.Infra.Repositories;
using Condom.Infra.Repositories.Identity;
using Condom.Infra.Sender;
using Condom.Infra.Validations;
using Condom.Infra.Validations.Base;
using Condom.Views.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra
{

    public static class IOC
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<MailSender>();

            services.AddPooledDbContextFactory<CondomContext>(optionsAction: (o) =>
            {
                o.UseLoggerFactory(LoggerFactory.Create(config =>
                {
                    config.AddConsole();
                }));
                o.EnableSensitiveDataLogging(true);
                o.EnableDetailedErrors(true);
                o.EnableSensitiveDataLogging(true);
                //Se tirar isso, terá que lembrar de sempre fazer os selects por EF com AsNoTracking()
                o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                o.UseSqlServer(Global.CondomResources.DB, (sqlO) =>
                {
                    sqlO.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
            });

            services.AddTransient<SqlConnection>(e => new SqlConnection(Global.CondomResources.DB));

            services.AddTransient<CondomContext>(p => p.GetRequiredService<IDbContextFactory<CondomContext>>().CreateDbContext());


            services.AddScoped<SignInManager<Users>>();
            services.AddScoped<UserManager<Users>>();
            services.AddScoped<IIdentityRoleStore<Roles>, IdentityRoleStore>();
            services.AddScoped<IIdentityUserStore<Users>, IdentityUserStore>();

            services.Configure<SecurityStampValidatorOptions>(o => o.ValidationInterval = TimeSpan.FromMinutes(1));

            services.AddIdentity<Users, Roles>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

            }).AddEntityFrameworkStores<CondomContext>()
              .AddClaimsPrincipalFactory<CustomClaimFactory>()
              .AddRoleStore<IdentityRoleStore>()
              .AddUserStore<IdentityUserStore>()
              .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/account/login";
                options.AccessDeniedPath = "/Identity/Account/AccessDenied";
                options.SlidingExpiration = true;
            });

            //services.AddTransient<IdentityUserEmailStore>();
            //services.AddTransient<IdentityUserLoginStore>();
            //services.AddTransient<IdentityUserPasswordStore>();
            //services.AddTransient<IdentityUserRoleStore>();
            //services.AddTransient<IdentityUserStore>();


            services.AddScoped<UserValidator>();
            services.AddScoped<IdentityApp>();
            services.AddScoped<UserProfilesStore>();

            services.AddScoped<GroupValidator>();
            services.AddScoped<GroupsApp>();
            services.AddScoped<GroupsStore>();

        }
    }
}
