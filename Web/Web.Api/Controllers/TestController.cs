using System.Web.Http;

namespace Web.Api.Controllers
{
    [RoutePrefix("")]
    public class TestController : ApiController
    {
        [HttpGet, Route("")]
        public IHttpActionResult Test()
        {
            return Ok("API is okidoki");
        }

        [Authorize, HttpGet, Route("access")]
        public IHttpActionResult Access()
        {
            return Ok("You've got access");
        }
    }
}
