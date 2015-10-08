using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Http.Routing;
using IdentityModel.Client;
using Web.Api.Models.Authenticate;

namespace Web.Api.Controllers
{
    [RoutePrefix("Authenticate")]
    public class AuthenticateController : ApiController
    {
        [HttpPost, Route("")]
        public async Task<IHttpActionResult> Authenticate(AuthenticateRequest authenticateRequest)
        {
            var tokenClient = new TokenClient("https://localhost:44302/connect/token", "AngularClient", "secret");

            var result = await tokenClient.RequestResourceOwnerPasswordAsync(authenticateRequest.Username,authenticateRequest.Password, "Api"); //offline_access scope enables refresh token
            return Ok(new
            {
                result.AccessToken,
                result.RefreshToken
            });
        }

        [HttpPost, Route("refresh-token")]
        public async Task<IHttpActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var tokenClient = new TokenClient("https://localhost:44302/connect/token", "AngularClient", "secret");

            var result = await tokenClient.RequestRefreshTokenAsync(refreshTokenRequest.RefreshAccessToken);

            return Ok(new
            {
                result.AccessToken,
                result.RefreshToken
            });
        }

        [HttpGet, Route("external-login")]
        public IHttpActionResult ExternalLogin()
        {
            var state = Guid.NewGuid().ToString("N");
            var nonce = Guid.NewGuid().ToString("N");
            //SetTempState(state, nonce);

            var request = new AuthorizeRequest("https://localhost:44302/connect/authorize");

            var url = request.CreateAuthorizeUrl(
                clientId: "AngularClient2",
                responseType: "code",
                scope: "Api",
                redirectUri: Url.Link("externalLoginCallBack", new {} ),
                state: state,
                nonce: nonce,acrValues:"idp:Facebook");

            return Redirect(url);
        }

        [HttpGet, Route("external-login-callback", Name = "externalLoginCallBack")]
        public IHttpActionResult ExternalLoginCallback()
        {
            return Ok();
        }
    }
}
