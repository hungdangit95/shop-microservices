using Microsoft.AspNetCore.Mvc;

namespace Customer.API.Controllers;

public class HomeController : ControllerBase
{
    // GET
    public IActionResult Index()
    {
        return Redirect("~/swagger");
    }
}