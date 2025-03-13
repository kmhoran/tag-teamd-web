using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace TagTeamdWeb.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        [Authorize(Policy = "ValidUser")]
        public IActionResult HelloWorld()
        {
            Console.WriteLine("Hello, World called");

            return Ok(new { message = "Hello, World" });
        }

        [HttpPost("echo")]
        [Authorize(Policy = "ValidUser")]
        public IActionResult Echo([FromBody] string message)
        {
            return Ok(new { m = message });
        }
    }
}