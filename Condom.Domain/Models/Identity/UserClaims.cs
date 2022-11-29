
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models
{
    public class UserClaims : Microsoft.AspNetCore.Identity.IdentityUserClaim<Guid>, IBaseModel<UserClaims>, IEntity
    {
        public UserClaims()
        {
        }
        public UserClaims(UserClaims domain)
        {
            Overwrite(domain);
        }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserClaims>(entity =>
            {
                entity.ToTable(name: "UserClaims");
            });
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(UserClaims domain)
        {
            throw new NotImplementedException();
        }
    }
}
