﻿using Condom.Domain.Global;
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
    public partial class RoleClaim : IdentityRoleClaim<Guid>, IBaseModel<RoleClaim>, IEntity
    {
        public RoleClaim()
        {
        }

        public RoleClaim(RoleClaim domain)
        {
            Overwrite(domain);
        }

        public void CleanReferences()
        {
            throw new NotImplementedException();
        }

        public void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable(name: "RoleClaim");
            });

        }

        public TEntity GetClone<TEntity>() where TEntity : class
        {
            throw new NotImplementedException();
        }

        public void Overwrite(RoleClaim domain)
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
