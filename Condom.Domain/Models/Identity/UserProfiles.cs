using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models.Identity
{
    public class UserProfiles : BaseModel<UserProfiles>
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }    
        public string State { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Number { get; set; }

        public override void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserProfiles>(x =>
            {
                x.ToTable("UserProfiles");

                x.HasKey(x => x.UserId);

                x.Property(x => x.UserId).IsRequired();

                x.Property(x => x.Name).IsRequired().HasColumnType("varchar(100)");

                x.Property(x => x.State).IsRequired().HasColumnType("varchar(60)");

                x.Property(x => x.City).IsRequired().HasColumnType("varchar(70)");

                x.Property(x => x.Address).IsRequired().HasColumnType("varchar(200)");

                x.Property(x => x.Number).IsRequired().HasColumnType("varchar(10)");
            });
        }

        public override void Overwrite(UserProfiles domain)
        {
            throw new NotImplementedException();
        }
    }
}
