using Condom.Domain.Global;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Models
{
    public class UserRoles : Microsoft.AspNetCore.Identity.IdentityUserRole<Guid>, IBaseModel<UserRoles>, IEntity
    {
        public UserRoles()
        {
        }

        public UserRoles(UserRoles domain)
        {
            Overwrite(domain);
        }

        public virtual Users User { get; set; }
        public virtual Roles Role { get; set; }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRoles>(entity =>
            {
                entity.ToTable("UserRoles");

                entity.HasOne(x => x.User)
                      .WithMany(x => x.UserRoles)
                      .HasForeignKey(x => x.UserId)
                      .HasPrincipalKey(x => x.Id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Role)
                      .WithMany(x => x.UserRoles)
                      .HasForeignKey(x => x.RoleId)
                      .HasPrincipalKey(x => x.Id)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(UserRoles  domain)
        {
            throw new NotImplementedException();
        }
        CrudEnum Crud { get; set; }
        Tracker _Tracker { get; set; } = new Tracker();
        public CrudEnum GetCrud()
        {
            return Crud;
        }

        public Tracker GetTracker()
        {
            return _Tracker;
        }

        public void SetCrud(CrudEnum crud)
        {
            Crud = crud;
        }
    }
}
