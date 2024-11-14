using Microsoft.AspNetCore.Mvc;
using RestaurantReservation.API.Interfaces;
using RestaurantReservation.API.Services;

namespace RestaurantReservation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IJwtTokenGenerator _jwtTokenGenerator;

        public AuthenticationController(IJwtTokenGenerator jwtTokenGenerator)
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

