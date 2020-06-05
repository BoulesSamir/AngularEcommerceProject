using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SouqyApi.Models
{
    public class ApplicationUser:IdentityUser
    {
       public string FullName { get; set; }
        public string Address { get; set; }
        public virtual ICollection<Product>Products { get; set; }
}
}