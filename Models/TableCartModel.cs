using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EddsWaffle.Models
{
    public class TableCartModel
    {
        public string code_product { get; set; }
        public string name_product { get; set; }
        public decimal quantity_product { get; set; }
        public decimal price_product { get; set; }
        public decimal totalprice_product { get; set; }
    }
}