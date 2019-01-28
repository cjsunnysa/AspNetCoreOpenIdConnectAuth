using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreSecurityPrototype.Data;
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
        private readonly AuthPrototypeContext _dbContext;

        public SecurityController(AuthPrototypeContext dbContext)
        {
            this._dbContext = dbContext;
        }

        [HttpGet("users")]
        public ActionResult<IEnumerable<string>> GetUsers()
        {
            return 
                _dbContext
                    .Contact
                    .Select(c => $"{c.FirstName} {c.LastName}")
                    .ToArray();
        }
    }
}
