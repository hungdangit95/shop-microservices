using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Redirect("~/swagger");
        }
    }
}