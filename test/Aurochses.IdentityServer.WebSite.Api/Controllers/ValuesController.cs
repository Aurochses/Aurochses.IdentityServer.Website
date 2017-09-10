using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Aurochses.IdentityServer.WebSite.Api.Controllers
{
    /// <summary>
    /// Values
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new [] { "value1", "value2" };
        }

        /// <summary>
        /// Get by id
        /// </summary>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "You are Authorized.";
        }

        /// <summary>
        /// Get Policy
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize("ValueRead")]
        [HttpGet("claim")]
        public string GetPolicy(int id)
        {
            return "You are Authorized and have correct claims.";
        }
    }
}