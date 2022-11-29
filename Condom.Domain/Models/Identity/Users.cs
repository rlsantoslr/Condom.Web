using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models
{
    public class Users : IdentityUser<Guid>, IBaseModel<Users>, IEntity
    {
        public Users()
        {
        }

        public Users(Users domain)
        {
            Overwrite(domain);
        }

        public virtual ICollection<UserClaims> Claims { get; set; }
        public virtual ICollection<UserLogins> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            {
                modelBuilder.Entity<Users>(b =>
                {
                    b.ToTable("Users");

                    b.Property(c => c.Email)
                        .HasColumnType("varchar(32)")
                        .HasMaxLength(32)
                        .IsRequired();

                    // Each User can have many UserClaims
                    b.HasMany(e => e.Claims)
                        .WithOne()
                        .HasForeignKey(uc => uc.UserId)
                        .IsRequired();

                    // Each User can have many UserLogins
                    b.HasMany(e => e.Logins)
                        .WithOne()
                        .HasForeignKey(ul => ul.UserId)
                        .IsRequired();

                    // Each User can have many UserTokens
                    b.HasMany(e => e.Tokens)
                        .WithOne()
                        .HasForeignKey(ut => ut.UserId)
                        .IsRequired();
                });
            }
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(Users domain)
        {
            throw new NotImplementedException();
        }
    }
}
