using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SouqyApi.DTO;
using SouqyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;

namespace SouqyApi.Controllers
{
    public class UserController : ApiController
    {
        [Authorize]
     public async Task<IHttpActionResult> getUserName()
        {
            string userName = ClaimsPrincipal.Current.Identity.GetUserName();
            if (userName != null)
            {
                return Ok(userName);
            }
            else
            {
                return BadRequest("Not logged");
            }
        }
        public async Task<IHttpActionResult> registration(Usermodel account)
        {
            UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(new ApplicationDbContext());
            UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(store);
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            else
            {
                try
                {
                    ApplicationUser user = new ApplicationUser();
                    user.UserName = account.UserName;
                    user.PasswordHash = account.Password;
                    user.Email = account.Email;
                    user.FullName = account.FullName;
                    user.Address = account.Address;
                    IdentityResult result = await userManager.CreateAsync(user, user.PasswordHash);
                    if (result.Succeeded)
                    {
                        return Created("", "created success" + user.UserName);
                    }
                    else
                    {
                        string errors = "";
                        foreach (var item in result.Errors.ToList())
                        {
                            errors += item + "\n";
                        }

                        return BadRequest(errors);
                    }

                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
        }
    }
}
