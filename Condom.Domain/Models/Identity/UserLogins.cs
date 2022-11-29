
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models
{
    public class UserLogins : Microsoft.AspNetCore.Identity.IdentityUserLogin<Guid>, IBaseModel<UserLogins>, IEntity
    {
        public UserLogins()
        {
        }
        public UserLogins(UserLogins domain)
        {
            Overwrite(domain);
        }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserLogins>(entity =>
            {
                entity.ToTable("UserLogins");
            });
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(UserLogins domain)
        {
            throw new NotImplementedException();
        }
    }
}
