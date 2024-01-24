using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using TFA.Domain.UseCases.GetForums;


namespace TFA.API.Controllers
{
    [ApiController]
    [Route("forums")]
    public class ForumController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Models.Forum[]))]
        public async Task<IActionResult> GetForums([FromServices] IGetForumsUseCase useCase, CancellationToken cancellationToken)
        {
            var forums = await useCase.Execute(cancellationToken);            
            
             return Ok(forums);
        }
    }
}