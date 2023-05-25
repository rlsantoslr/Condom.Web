using Condom.Domain.Models;
using Condom.Infra.App;
using Condom.Infra.Claims;
using Condom.Infra.Context;
using Condom.Infra.Repositories.Identity;
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

            services.AddScoped<IIdentityRoleStore<Roles>, IdentityRoleStore>();
            services.AddScoped<IIdentityUserStore<Users>, IdentityUserStore>();


            services.AddIdentity<Users, Roles>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            }).AddEntityFrameworkStores<CondomContext>()
              .AddClaimsPrincipalFactory<CustomClaimFactory>()
              .AddRoleStore<IdentityRoleStore>()
              .AddUserStore<IdentityUserStore>();

            //services.AddTransient<IdentityUserEmailStore>();
            //services.AddTransient<IdentityUserLoginStore>();
            //services.AddTransient<IdentityUserPasswordStore>();
            //services.AddTransient<IdentityUserRoleStore>();
            //services.AddTransient<IdentityUserStore>();


            services.AddScoped<UserValidator>();
            services.AddScoped<IdentityApp>();
        }
    }
}
