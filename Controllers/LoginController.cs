
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace JwtInDotnetCore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Login Jwt (POSTMAN)
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// ```json
        /// POST /api/Login 
        /// {
        ///     "email": "testUser",
        ///     "password": "testPassword"
        /// }
        /// ```
        /// </remarks>
        /// <param name="loginRequest"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] LoginRequest loginRequest)
        {
            //your login for process
            //if login username and password are correct then proceed to generate token
            if (loginRequest.Email == "testUser" && loginRequest.Password == "testPassword")
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var Sectoken = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                null,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

                var token = new JwtSecurityTokenHandler().WriteToken(Sectoken);

                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }
    }
}