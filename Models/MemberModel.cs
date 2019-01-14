using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EddsWaffle.Models
{
    public class MemberModel
    {
        public string code_member { get; set; }
        public string name_member { get; set; }
        public System.DateTime borndate_member { get; set; }
        public string gender_member { get; set; }
        public decimal phone_member { get; set; }
        public string address_member { get; set; }
    }
}