using Crud.Business;
using Crud.Business.Managers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CrudApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private const string tokenKey = "UnSecuryKeyForCrudAppExcersice";
        private readonly TimeSpan tokenLifeTime = TimeSpan.FromHours(8);
        private readonly ILogger<UserController> _logger;
        private readonly IUserManager _userManager;
        public UserController(ILogger<UserController> logger, IUserManager userManager)
        {
            _logger = logger;
            _userManager = userManager;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var credentials = await _userManager.GetUser(email, password);
            if (credentials == null)
            {
                return NotFound();
            }
            var key = Encoding.UTF8.GetBytes(tokenKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim> {
                new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new (JwtRegisteredClaimNames.Sub, credentials.Email) ,
                new (JwtRegisteredClaimNames.Email, credentials.Email) ,
                new ("userid", credentials.IdUser.ToString())
            };

            var tokeDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.Add(tokenLifeTime),
                Issuer = "https://crud.app.com",
                Audience = "https://crud.book.app.com",
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokeDescriptor);
            var jwt = tokenHandler.WriteToken(token);
            return Ok(jwt);
        }

        [HttpPost]
        public async Task<User> CreateUser(User user)
        {
            return await _userManager.CreateUser(user);
        }
    }
}
