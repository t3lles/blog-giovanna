using Microsoft.AspNetCore.Mvc;

namespace BlogGiovanna.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {
        [HttpGet ("")]
        public IActionResult Get()
        {
            return Ok();
        }

    }
}
