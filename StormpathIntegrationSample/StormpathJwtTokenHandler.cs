using Stormpath.SDK;
using Stormpath.SDK.Application;
using Stormpath.SDK.Client;
using Stormpath.SDK.Oauth;
using System.IdentityModel.Tokens;
using System.Security.Claims;

namespace StormpathIntegrationSample
{
    internal class StormpathJwtTokenHandler : JwtSecurityTokenHandler
    {
        private readonly IApplication _stormPathApplication;

        public StormpathJwtTokenHandler()
        {
            IClient client = Clients.Builder()
                                   .SetApiKeyFilePath("apiKey.properties")
                                   .Build();

            _stormPathApplication = client.GetApplications().Where(s => s.Name == "stormpath").FirstOrDefaultAsync().Result;
        }

        public override ClaimsPrincipal ValidateToken(string securityToken, 
            TokenValidationParameters validationParameters, 
            out SecurityToken validatedToken)
        {
            var jwtAuthenticationRequest = OauthRequests.NewJwtAuthenticationRequest()
                .SetJwt(securityToken)
                .Build();

            IAccessToken validAccessToken = _stormPathApplication.NewJwtAuthenticator()
                .WithLocalValidation()
                .AuthenticateAsync(jwtAuthenticationRequest).Result;

            if (validAccessToken == null)
            {
                throw new SecurityTokenException("Invalid token");
            }

            var account = validAccessToken.GetAccountAsync().Result;

            ClaimsIdentity objClaim = new ClaimsIdentity("jwt");
            objClaim.AddClaim(new Claim(ClaimTypes.NameIdentifier, account.Username));
            //Use Email for Name instead FullName
            objClaim.AddClaim(new Claim(ClaimTypes.Name, account.Email));
            objClaim.AddClaim(new Claim(ClaimTypes.Email, account.Email));
            objClaim.AddClaim(new Claim(ClaimTypes.Role, "user"));

            validatedToken = ReadToken(validAccessToken.Jwt);
            var principal = new ClaimsPrincipal(objClaim);

            return principal;
        }
    }
}