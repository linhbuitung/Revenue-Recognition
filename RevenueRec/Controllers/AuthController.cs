using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using RevenueRec.Context;
using RevenueRec.Helpers;
using RevenueRec.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RevenueRec.RequestDtos;
using RevenueRec.ResponseDtos;

namespace RevenueRec.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly s28786Context _context;

        public AuthController(IConfiguration configuration, s28786Context context)
        {
            _configuration = configuration;
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult RegisterUser(RegisterUserDto model)
        {
            var hashedPasswordAndSalt = SecurityHelpers.GetHashedPasswordAndSalt(model.Password);
            Employee userTemp = _context.Employees.Where(u => u.Username == model.Username).FirstOrDefault();

            if (userTemp != null)
            {
                return Conflict("User already exists");
            }

            var user = new Employee()
            {
                Username = model.Username,
                Password = hashedPasswordAndSalt.Item1,
                Role = model.Role,
                Salt = hashedPasswordAndSalt.Item2,
                RefreshToken = SecurityHelpers.GenerateRefreshToken(),
                RefreshTokenExp = DateTime.Now.AddDays(1)
            };

            _context.Employees.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        //for test
        [Authorize]
        [HttpGet]
        public IActionResult GetUsers()
        {
            var claimsFromAccessToken = User.Claims;
            return Ok("Secret data");
        }

        //for test
        [AllowAnonymous]
        [HttpGet("anon")]
        public IActionResult GetAnonData()
        {
            return Ok("Public data");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login(LoginUserDto loginRequest)
        {
            Employee user = _context.Employees.Where(u => u.Username == loginRequest.Username).FirstOrDefault();
            //test print to console
            Console.WriteLine("Role: " + user.Role);
            if (user == null)
            {
                return NotFound("Wrong username");
            }
            string passwordHashFromDb = user.Password;
            string curHashedPassword = SecurityHelpers.GetHashedPasswordWithSalt(loginRequest.Password, user.Salt);

            if (passwordHashFromDb != curHashedPassword)
            {
                return Unauthorized();
            }

            Claim[] userclaim = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
            user.RefreshTokenExp = DateTime.Now.AddDays(1);
            _context.SaveChanges();
            TokenResponseDto res = new TokenResponseDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = user.RefreshToken
            };
            return Ok(res);
        }

        [Authorize(AuthenticationSchemes = "IgnoreTokenExpirationScheme")]
        [HttpPost("refresh")]
        public IActionResult Refresh(RefreshTokenRequestDto refreshToken)
        {
            Employee user = _context.Employees.Where(u => u.RefreshToken == refreshToken.RefreshToken).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid refresh token");
            }

            if (user.RefreshTokenExp < DateTime.Now)
            {
                throw new Exception("Refresh token expired");
            }

            Claim[] userclaim = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            };

            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["SecretKey"]));

            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtToken = new JwtSecurityToken(
                issuer: "https://localhost:5001",
                audience: "https://localhost:5001",
                claims: userclaim,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: creds
            );

            user.RefreshToken = SecurityHelpers.GenerateRefreshToken();
            user.RefreshTokenExp = DateTime.Now.AddDays(1);

            _context.SaveChanges();

            TokenResponseDto res = new TokenResponseDto()
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                RefreshToken = user.RefreshToken
            };

            return Ok(res);
        }
    }
}