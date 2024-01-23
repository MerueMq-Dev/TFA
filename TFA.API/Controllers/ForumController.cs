using Microsoft.AspNetCore.Mvc;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForumController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetName()
        {
            return Ok("name");
        }
    }
}