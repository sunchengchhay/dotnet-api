using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Product
{
    public class PostProductDto
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }
    }
}