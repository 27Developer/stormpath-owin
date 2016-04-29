using Microsoft.Owin;
using Stormpath.SDK;
using Stormpath.SDK.Account;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;
using Stormpath.SDK.Oauth;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace StormpathIntegrationSample
{
    [RoutePrefix("api")]
    public class DefaultController : ApiController
    {
        private readonly IApplication _stormpathApplication;

        public DefaultController()
        {
            IClient client = Clients.Builder()
                                    .SetApiKeyFilePath("apiKey.properties")
                                    .Build();

            _stormpathApplication = client.GetApplications().Where(s => s.Name == "stormpath").FirstOrDefaultAsync().Result;
        }

        [Route("time")]
        public async Task<DateTime> GetServerTime()
        {
            return await Task.FromResult(DateTime.Now);
        }

        [Authorize]
        [Route("securetime")]
        public async Task<string> GetSecureServerTime()
        {
            return await Task.FromResult("Secured Time: " + DateTime.Now.ToShortTimeString());
        }

        [Route("login")]
        public async Task<string> Login(LoginModel loginValues)
        {
            var passwordGrantRequest = OauthRequests.NewPasswordGrantRequest()
                    .SetLogin(loginValues.UserName)
                    .SetPassword(loginValues.Password)
                    .Build();

            var grantResult = await _stormpathApplication.NewPasswordGrantAuthenticator()
                .AuthenticateAsync(passwordGrantRequest);

            var accessToken = await grantResult.GetAccessTokenAsync();

            return accessToken.Jwt;
        }
    }
}
