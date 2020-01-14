using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromHeader]string authorization, [FromBody]UserModel login)
        {
            string encodedUsernamePassword = authorization.Substring("Basic ".Length).Trim();
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            string cient_id_secret = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = cient_id_secret.IndexOf(':');
            var client_id = cient_id_secret.Substring(0, seperatorIndex);
            var client_secret = cient_id_secret.Substring(seperatorIndex + 1);

            IActionResult response = Unauthorized();
            if (client_id == _config["Jwt:client_id"] && client_secret == _config["Jwt:client_secret"])
            {
                var user = AuthenticateUser(login);

                if (user != null)
                {
                    var tokenString = GenerateJSONWebToken(user);
                    response = Ok(new { token = tokenString });
                }
            }

            return response;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Username),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim("DateOfJoing", userInfo.DateOfJoing.ToString("yyyy-MM-dd")),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel AuthenticateUser(UserModel login)
        {
            UserModel user = null;

            if (login.Username == "Vijay")
            {
                user = new UserModel
                {
                    Username = "Vijay Reddy",
                    EmailAddress = "vijayreddy@gmail.com",
                    DateOfJoing = new DateTime(2010, 08, 02)
                };
            } else if (login.Username == "Don")
            {
                user = new UserModel
                {
                    Username = "Donthireddy",
                    EmailAddress = "donthireddy@gmail.com",
                    DateOfJoing = new DateTime(2018, 08, 02)
                };
            }
            return user;
        }
    }
}