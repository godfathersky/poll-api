using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PollAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PollAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly PollAppDBContext _context;

        public AuthController(IConfiguration configuration, PollAppDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> Register (UserRegister userRegRequest)
        {
            User user = new User();
            CreatePasswordHash(userRegRequest.Password, out byte[] UserPasswordHash, out byte[] UserPasswordSalt);

            String currentTime = DateTime.Now.ToString("HH:mm:ss");
            TimeSpan currentTimeToDb = TimeSpan.Parse(currentTime);

            user.UserLogin = userRegRequest.Username;
            user.UserPasswordHash = UserPasswordHash;
            user.UserPasswordSalt = UserPasswordSalt;
            user.UserAddTime = currentTimeToDb;
            user.UserEmail = userRegRequest.Email;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(user);
        }

        [HttpGet("{username}")]
        public async Task<ActionResult<IEnumerable<User>>> GetUser (string username)
        {
            var user = await _context.Users.Where(x => x.UserLogin == username).ToListAsync();

            return user;
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login (UserLogin UserLogRequest)
        {
            var userGet = await GetUser(UserLogRequest.Username);

            var accUser = userGet.Value.First();

            if (accUser.UserLogin != UserLogRequest.Username)
            {
                return BadRequest();
            }

            if (!VerifyPasswordHash(UserLogRequest.Password, accUser.UserPasswordHash, accUser.UserPasswordSalt))
            {
                return BadRequest();
            }

            string token = CreateToken(accUser);

            return token;
        }

        [HttpGet]
        [Route("userrole/{id}")]
        public async Task<ActionResult<List<Role>>> GetUserRoles(int id)
        {
            var role = await _context.Roles.Where(x => x.RoleId == id).ToListAsync();

            return role;
        }

        private string CreateToken(User user)
        {
            List<int> userRolesList = new List<int>();
            var userRoles = user.UserRoles;
            foreach (var x in userRoles)
            {
                userRolesList.Add(x.RoleId);
            }
            List<string> userRoleName = new List<string>();
            foreach (var x in userRolesList)
            {
                var userRoleGet = GetUserRoles(x);
                var userFirstRole = userRoleGet.Result.Value.First().RoleName;
                userRoleName.Add(userFirstRole);
            }

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.UserLogin));

            foreach (var x in userRoleName)
            {
                claims.Add(new Claim(ClaimTypes.Role, x.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        private void CreatePasswordHash (string password, out byte[] UserPasswordHash, out byte[] UserPasswordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                UserPasswordSalt = hmac.Key;
                UserPasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash (string password, byte[] UserPasswordHash, byte[] UserPasswordSalt)
        {
            using (var hmac = new HMACSHA512(UserPasswordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(UserPasswordHash);
            }
        }
    }
}
