using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTProject.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace JWTProject.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        public AuthenticateController(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
          _userManager = userManager;
           _roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier,user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(14),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            }
            else
            {
                await _userManager.AddToRoleAsync(user,model.Role);
                return Ok(new Response { Status = "Success", Message = "User created successfully!" });
            }
           
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task <IActionResult> CreateRole([FromBody] RoleModel model)
        {
            if (model.RoleName != null)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };
                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Role created successfully!" });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role creation failed! Please check Role details and try again." });
            }
            return Ok(new Response { Status = "Error", Message = "Please Add Role" });

        }


        [HttpDelete]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole([FromBody] RoleModel model)
        {
            if (model.RoleName != null)
            {

                IdentityRole identityRole = await _roleManager.FindByNameAsync(model.RoleName);
                var result = await _roleManager.DeleteAsync(identityRole);
                if (result.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Role Deleted successfully!" });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Role Deleted failed! Please check Role details and try again." });
            }
            return Ok(new Response { Status = "Error", Message = "Please Add Role" });
        }

        [HttpPut]
        [Route("EditUser")]
        public async Task<IActionResult> EditUser([FromBody] RegisterModel model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var userLogin = await _userManager.FindByIdAsync(userId);

            userLogin.Email = model.Email;
            userLogin.UserName = model.Username;
            var newPassword = _userManager.PasswordHasher.HashPassword(userLogin, model.Password);
            userLogin.PasswordHash = newPassword;

                var result = await _userManager.UpdateAsync(userLogin);

                if (result.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Edit User successfully!" });
                }
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Edit User failed! Please check Role details and try again." });
        }

        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser([FromBody] LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(new Response { Status = "Success", Message = "Deleted User successfully!" });
                }
            }
            return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Deleted User failed! Please check Role details and try again." });
        }

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            var Users = _userManager.Users;
            return Ok(Users);
        }
    }   
}