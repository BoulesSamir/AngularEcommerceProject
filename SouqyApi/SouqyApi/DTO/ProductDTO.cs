using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SouqyApi.DTO
{
    public class ProductDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int quantity { get; set; }
         public double price { get; set; }
        public  string img { get; set; }
        public string Description { get; set; }
        public string OwnerName { get; set; }
    }
}