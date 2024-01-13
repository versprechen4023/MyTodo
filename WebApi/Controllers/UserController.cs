using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin, User")]
    public class UserController : ControllerBase
    {
        [HttpGet("useraction")]
        public async Task<IActionResult> UserAction()
        {
            return Ok();
        }
    }
}
