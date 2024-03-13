
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniProjectBack.ModelsAuth;
using PortalVioo.Interface;
using PortalVioo.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController(IRepositoryGenericApp<ApplicationUser> repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
       
        private readonly IRepositoryGenericApp<ApplicationUser> _repository = repository;


        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {

            List<IdentityRole> roles = roleManager.Roles.ToList();

            return Ok(roles);
        }

        [HttpGet]
        [Route("GetUserByUsername")]
        public async Task<IActionResult> GetUserByUsername(string Username)
        {
            var user = await userManager.FindByNameAsync(Username);
            var userRoles = await userManager.GetRolesAsync(user);
            IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == userRoles[0].ToLower()).FirstOrDefault();
            return Ok(new
            {
                username = Username,
                email = user.Email,
                Role = roles.Name,
                RoleId = roles.Id,
            });
        }

        [HttpPost]
        [Route("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleModel cr)
        {
            var user = await userManager.FindByNameAsync(cr.username);
            var role = await userManager.GetRolesAsync(user);
            IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == role[0].ToLower()).FirstOrDefault();
            await userManager.RemoveFromRoleAsync(user, roles.Name);
            var result = await userManager.AddToRoleAsync(user, cr.role);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            { return Ok(cr.role); }

        }



        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel ch)
        {
            var user = await userManager.FindByNameAsync(ch.username);
            ch.token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassResult = await userManager.ResetPasswordAsync(user, ch.token, ch.password);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            { return Ok(ch.password); }
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            List<getuserwthrole> listUserWithRole = new List<getuserwthrole>();
            var listUser = _repository.GetAll(null, null);
            foreach (var user in listUser)
            {
                var role = await userManager.GetRolesAsync(user);
                IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == role[0].ToLower()).FirstOrDefault();
                getuserwthrole usrsRole = new getuserwthrole();
                usrsRole.UserId = user.Id;
                usrsRole.RoleId = roles.Id;
                usrsRole.Username = user.UserName;
                usrsRole.Email = user.Email;
                usrsRole.Role = roles.Name;
                listUserWithRole.Add(usrsRole);

            }
            return Ok(listUserWithRole);
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Username);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new(ClaimTypes.Name, user.UserName),
                    new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    Role = userRoles[0]
                });
            }
            return Unauthorized();
        }


        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model, string Role)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, Role);
            if (await roleManager.RoleExistsAsync(Role))
            {
                await userManager.AddToRoleAsync(user, Role);
            }
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(UserRoles.ADMIN))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.ADMIN));
            if (!await roleManager.RoleExistsAsync(UserRoles.DEV))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.DEV));
            if (!await roleManager.RoleExistsAsync(UserRoles.GESTIONNAIRE))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.GESTIONNAIRE));

            if (await roleManager.RoleExistsAsync(UserRoles.ADMIN))
            {
                await userManager.AddToRoleAsync(user, UserRoles.ADMIN);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
