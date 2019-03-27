using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JWTDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //метод возращаюший токен
        [HttpPost("token")]
        public IActionResult GetToken()
        {
            // security key
            var security_key = "this_is_very_powerfull_secret_key_2019";

            // symetrick security key
            var symetrickSecKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(security_key));

            // singing credentials
            var singingcredentials = new SigningCredentials(symetrickSecKey, SecurityAlgorithms.HmacSha256);

            // create token
            var token = new JwtSecurityToken(
                issuer:"http://www.evil.org",
                audience:"http://www.evil.org",
                expires:DateTime.Now.AddHours(1),
                signingCredentials:singingcredentials

                );

            //return token
            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}