using Condom.Domain.Global;
using Condom.Domain.Models.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Domain.Models
{
    public class Groups : BaseModel<Groups>
    {
        public Guid Id { get; set; }
        [MinLength(3)]
        [MaxLength(200)]
        [Required]
        [Display(Name="Nome do grupo")]
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        public string Name { get; set; }
        public Guid OwnerId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        [Required]
        [Display(Name="Categoria")]
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        public TypeEnum Type { get; set; }
        [MinLength(10)]
        [MaxLength(300)]
        [Required]
        [Display(Name = "URL da imagem")]
        [Url]
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        public string ImageURL { get; set; }
        public DateTime BuildAt { get; set; }

        public GroupBillings Billings { get; set; }

        public override void DBConfig(ref ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Groups>(x =>
            {
                x.ToTable("Groups");

                x.HasKey(x => x.Id);

                x.Property(x => x.Id).IsRequired();

                x.Property(x => x.Name).IsRequired().HasColumnType("varchar(200)");

                x.Property(x => x.OwnerId).IsRequired();

                x.Property(x => x.CreatedAt).IsRequired();

                x.Property(x => x.Type).IsRequired();

                x.Property(x => x.ImageURL).HasColumnType("varchar(300)");

                x.Property(x => x.BuildAt).IsRequired();

                x.Ignore(x => x.Billings);
            });
        }

        public override void Overwrite(Groups domain)
        {
            throw new NotImplementedException();
        }

        public enum TypeEnum
        {
            [Display(Name = "Condomínio de Casas")]
            Houses = 0,
            [Display(Name = "Condomínio de Apartamentos")]
            Apartament = 1
        }

    }
}
