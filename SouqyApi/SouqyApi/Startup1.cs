using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using SouqyApi.Models;

[assembly: OwinStartup(typeof(SouqyApi.Startup1))]

namespace SouqyApi
{
    public class Startup1
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(CorsOptions.AllowAll); app.UseOAuthAuthorizationServer(new OAuthAuthorizationServerOptions()
            {
                //URl http how expirestion
                TokenEndpointPath = new PathString("/login"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(60),
                AllowInsecureHttp = true,
                //how to create token (fields)==>
                Provider = new TokenCreate()

            });
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            HttpConfiguration config = new HttpConfiguration();

            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                "DefaultApi", "api/{controller}/{id}",
                new { id = RouteParameter.Optional });
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888
            app.UseWebApi(config);
        }
    }

    internal class TokenCreate : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {

            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials
           (OAuthGrantResourceOwnerCredentialsContext context)
        {

            context.OwinContext.Response.Headers.Add(" Access - Control - Allow - Origin ", new[] { "*" });

            UserStore<ApplicationUser> store =new UserStore<ApplicationUser>(new ApplicationDbContext());

            UserManager<ApplicationUser> manager =new UserManager<ApplicationUser>(store);
            IdentityUser user = await manager.FindAsync(context.UserName, context.Password);
            if (user == null)
            {
                context.SetError("grant_error", "username & password Not Valid");
            }
            else
            {

                ClaimsIdentity claims = new ClaimsIdentity(context.Options.AuthenticationType);

                claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                claims.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                context.Validated(claims);
            }


        }
    }
    }

