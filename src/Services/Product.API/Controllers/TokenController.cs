using Contracts.Identity;
using Microsoft.AspNetCore.Mvc;
using Shared.Dtos.Identity;

namespace Product.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        public TokenController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }
        [HttpGet]
        public IActionResult GetToken()
        {
            return Ok(_tokenService.GetToken(new TokenRequest()));
        }   
    }
}
