using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinalProject.Models
{
    public class Product
    {
        public Product()
        {
            this.Orders = new HashSet<Order>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<int> Unit { get; set; }
        public string Image { get; set; }
        public Nullable<int> CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}