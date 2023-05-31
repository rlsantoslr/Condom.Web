using Condom.Domain.Models;
using Condom.Domain.Models.Identity;
using Condom.Infra.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Infra.Context
{
    public class CondomContext : IdentityDbContext<Users, Roles, Guid, UserClaims, UserRoles, UserLogins, RoleClaim, UserToken>
    {
        public override DbSet<UserRoles> UserRoles { get; set; }
        public override DbSet<Roles> Roles { get; set; }
        public override DbSet<UserLogins> UserLogins { get; set; }
        public override DbSet<UserClaims> UserClaims { get; set; }
        public DbSet<UserToken> UserToken { get; set; }
        public DbSet<RoleClaim> RoleClaim { get; set; }
        public override DbSet<Users> Users { get; set; }
        public DbSet<UserProfiles> UserProfiles { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public CondomContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder o)
        {
            //ChangeTracker.LazyLoadingEnabled = false;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entities = Tools.GetAllEntities<IEntity>();
            foreach (var entity in entities)
            {
                if (entity.Key.Contains("Condom.Domain.Models"))
                {
                    Type? t = Type.GetType(entity.Value);
                    if (t != null)
                    {
                        var obj = Activator.CreateInstance(t) as IEntity;
                        obj.DBConfig(ref modelBuilder);
                    }

                }
            }

            //modelBuilder.HasChangeTrackingStrategy(ChangeTrackingStrategy.Snapshot);
        }
    }
}
