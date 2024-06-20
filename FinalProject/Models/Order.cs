using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Contact { get; set; }
        public string Address { get; set; }
        public int Unit { get; set; }
        public int Qty { get; set; }
        public int Total { get; set; }
        public  DateTime OrderDate { get; set; }
        public int InvoiceId { get; set; }
        public virtual Product Product { get; set; }
        public virtual Invoice Invoice { get; set; }
    }
}