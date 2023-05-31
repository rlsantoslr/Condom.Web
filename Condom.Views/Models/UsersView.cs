using Condom.Domain.Global;
using Condom.Domain.Models;
using Condom.Domain.Models.Identity;
using Condom.Views.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Condom.Domain.Global.CondEnum;

namespace Condom.Views.Models
{
    public class UsersView : BaseView<UsersView, Users>
    { 
        public UsersView()
        {
        }

        public UsersView(Users domain) : base(domain)
        {
        }

        public string Email { get => Domain.Email; set => Domain.Email = value; }

        [Validator(new CrudEnum[] { CrudEnum.Read, CrudEnum.Create })]
        [PasswordAttribute(5, 15)]
        [Required]
        public string Password { get; set; }

        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        public string ConfirmPassword { get; set; }

        public Guid UserId { get => GetProfile().UserId; set => GetProfile().UserId = value; }
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Name { get => GetProfile().Name; set => GetProfile().Name = value; }
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        [MinLength(2)]
        [MaxLength(60)]
        public string State { get => GetProfile().State; set => GetProfile().State = value; }
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        [MinLength(2)]
        [MaxLength(70)]
        public string City { get => GetProfile().City; set => GetProfile().City = value; }
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        [MinLength(2)]
        [MaxLength(200)]
        public string Address { get => GetProfile().Address; set => GetProfile().Address = value; }
        [Validator(new CrudEnum[] { CrudEnum.Create })]
        [Required]
        [MinLength(1)]
        [MaxLength(10)]
        public string Number { get => GetProfile().Number; set => GetProfile().Number = value; }

        public string Token { get; set; }

        protected UserProfiles GetProfile()
        {
            if(Domain.UserProfile == null)
            {
                Domain.UserProfile = new UserProfiles();
            }

            return Domain.UserProfile;
        }
    }
}
