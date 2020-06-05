using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SouqyApi.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int quantity { get; set; }
        public double price { get; set; }
        public string img { get; set; }
        public string Description { get; set; }
        public string ownerId { get; set; }
        [ForeignKey("ownerId")]
        public virtual ApplicationUser owner { get; set; }
    }
}