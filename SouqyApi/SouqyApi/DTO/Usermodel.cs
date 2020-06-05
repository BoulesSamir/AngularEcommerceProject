using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SouqyApi.DTO
{
    public class Usermodel
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }
}