using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Programming_Community_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Auth : ControllerBase
    {
        private static List<string> products = new List<string>
    {
        "Laptop",
        "Phone",
        "Tablet"
    };

        [HttpGet("all")]
        public IActionResult GetAll()
        {
            return Ok(products);
        }
        [HttpGet]
        public IActionResult message()
        {
            return Ok("hello");
        }
    }
}
