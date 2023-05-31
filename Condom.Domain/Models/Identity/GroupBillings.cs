using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Condom.Domain.Models.Identity
{
    public class GroupBillings
    {
        public Guid GroupId { get; set; }
        [ProtectedPersonalData]
        public string Name { get; set; }
        [ProtectedPersonalData]
        public string Surname { get; set; }
        [ProtectedPersonalData]
        public string FullAddress { get; set; }
        [ProtectedPersonalData]
        public string Email { get; set; }
        [ProtectedPersonalData]
        public string PhoneNumber { get; set; }
        [ProtectedPersonalData]
        public string CardName { get; set; }
        [ProtectedPersonalData]
        public string CardNumber { get; set; }
        [ProtectedPersonalData]
        public string CardExpiration { get; set; }
        [ProtectedPersonalData]
        public string CardSecurity { get; set; }

    }
}
