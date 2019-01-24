using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreSecurityPrototype.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        [HttpGet("users")]
        public ActionResult<IEnumerable<string>> GetUsers()
        {
            return new[]
            {
                "Gavin",
                "Danie",
                "Chris"
            };
        }
    }
}
