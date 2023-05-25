using Condom.Domain.Global;
using Condom.Domain.Models;
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

        [Validator(new CrudEnum[] { CrudEnum.Read })]
        [PasswordAttribute(5, 15)]
        [Required]
        public string Password { get; set; }
    }
}
