using Condom.Domain.Global;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Models
{
    public class Roles : IdentityRole<Guid>, IBaseModel<Roles>, IEntity 
    {
        public Roles(string roleName) : base(roleName)
        {
        }

        public Roles() : base()
        {
        }

        public Roles(Roles domain) : base()
        {
            Overwrite(domain);
        }

        public virtual ICollection<UserRoles> UserRoles { get; set; }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Roles>(x =>
            {
                x.ToTable("Roles");
            });
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(Roles domain)
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
