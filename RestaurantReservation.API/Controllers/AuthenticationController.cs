using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Services;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly JwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationController(JwtTokenGenerator jwtTokenGenerator)
        {
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("login")]
        public IActionResult Login(string username, string password)
        {

            var token = _jwtTokenGenerator.GenerateToken(username, password);
            return Ok(new { Token = token });

        }

    }
}

