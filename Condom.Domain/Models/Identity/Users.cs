using Condom.Domain.Global;
using Condom.Domain.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

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

        [Validator(new CrudEnum[] { CrudEnum.Read })]
        [EmailAddress]
        [Required]
        public override string Email { get; set; }

        public virtual ICollection<UserClaims> Claims { get; set; }
        public virtual ICollection<UserLogins> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRoles> UserRoles { get; set; }
        [NotMapped]
        public virtual UserProfiles UserProfile { get; set; }

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

                    b.Ignore(x => x.UserProfile);

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
            this.Email = domain.Email;
            this.LockoutEnd = domain.LockoutEnd;
            this.LockoutEnabled = domain.LockoutEnabled;
            this.UserProfile = domain.UserProfile;
            this.AccessFailedCount = domain.AccessFailedCount;
            this.ConcurrencyStamp = domain.ConcurrencyStamp;
            this.EmailConfirmed = domain.EmailConfirmed;
            this.Id = domain.Id;
            this.NormalizedEmail = domain.NormalizedEmail;
            this.NormalizedUserName = domain.NormalizedUserName;
            this.PasswordHash = domain.PasswordHash;
            this.PhoneNumber = domain.PhoneNumber;
            this.PhoneNumberConfirmed = domain.PhoneNumberConfirmed;
            this.SecurityStamp = domain.SecurityStamp;  
            this.TwoFactorEnabled  = domain.TwoFactorEnabled;
            this.UserName = domain.UserName;

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
