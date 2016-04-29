using Microsoft.Owin.Security.Jwt;
using Owin;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace StormpathIntegrationSample
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            // Configure Web API for self-host. 
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            var options = new JwtBearerAuthenticationOptions()
            {
                AllowedAudiences = new[] { "*" },
                TokenValidationParameters = new TokenValidationParameters(),
                TokenHandler = new StormpathJwtTokenHandler()
            };

            appBuilder.UseJwtBearerAuthentication(options);

            appBuilder.UseWebApi(config);
        }
    }
}
