﻿using System.Threading.Tasks;
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
    }
}
