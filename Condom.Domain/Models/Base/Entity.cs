using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models
{
    public interface IEntity
    {
        void DBConfig(ref ModelBuilder modelBuilder);
        void CleanReferences();
        TEntity GetClone<TEntity>() where TEntity : class;
    }

    public abstract class Entity : IEntity
    {
        public abstract void DBConfig(ref ModelBuilder modelBuilder);
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
                return true;

            if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
                return false;

            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }
        public virtual void CleanReferences()
        {
        }
        public TEntity GetClone<TEntity>() where TEntity : class
        {
            return (TEntity)MemberwiseClone();
        }
        public override string ToString()
        {
            return GetType().Name;
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
