using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // Foreign Key
        public int ProductId { get; set; }

        //Navigation property
        public Product Product { get; set; }
    }
}