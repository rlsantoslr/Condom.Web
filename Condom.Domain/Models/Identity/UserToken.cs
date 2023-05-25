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
    public class UserToken : IdentityUserToken<Guid>, IBaseModel<UserToken>, IEntity
    {
        public UserToken()
        {
        }
        public UserToken(UserToken domain)
        {
            Overwrite(domain);
        }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserToken>(entity =>
            {
                entity.ToTable("UserToken");
            });
        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(UserToken domain)
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
