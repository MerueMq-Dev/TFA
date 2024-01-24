using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TFA.Storage;

namespace TFA.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ForumController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(string[]))]
        public async Task<IActionResult> GetForums([FromServices] ForumDbContext context, CancellationToken cancellationToken)
        {
             var forumTitles = await context.Forums.Select(f => f.Title).ToArrayAsync(cancellationToken);

             return Ok(forumTitles);
        }
    }
}