// UserController.cs

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly string _jwtSecretKey ;

        public UserController(UserContext context,IConfiguration configuration)
        {
            _context = context;
              _jwtSecretKey = configuration["JwtSettings:SecretKey"];
              
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] User user)
        {
            try
            {
                // Validate user data
                if (user == null)
                {
                    return BadRequest("User data is invalid.");
                }

                // Check if the user already exists
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
                if (existingUser != null)
                {
                    return Conflict("User with this email already exists.");
                }

                // Hash the password before saving
                user.Password = HashPassword(user.Password);

                // Add user to the context and save changes
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Generate JWT token
                var token = JwtTokenGenerator.GenerateJwtToken(user.Id.ToString(), user.Username, user.Email, _jwtSecretKey);

                // Return success with user ID and token
                return Ok(new { UserId = user.Id, Token = token,UserName=user.Username });
            }
            catch (Exception ex)
            {   
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        [HttpPost("signin")]
        public IActionResult SignIn([FromBody] User user)
        {
            try
            {
                // Validate user credentials
                var existingUser = _context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == HashPassword(user.Password));
                if (existingUser == null)
                {
                    return Unauthorized("Invalid email or password.");
                }

                // Generate JWT token
                var token = JwtTokenGenerator.GenerateJwtToken(existingUser.Id.ToString(), existingUser.Username, existingUser.Email, _jwtSecretKey);

                // Return success with user ID and token
                return Ok(new { UserId = existingUser.Id, Token = token ,UserName=existingUser.Username ,UserEmail=existingUser.Email});
            }
            catch (Exception ex)
            {
                // Log the error
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }

        private string HashPassword(string password)
        {
            // Implement your password hashing logic here
            return password; // For demo purposes, return plain password
        }
    }
}
