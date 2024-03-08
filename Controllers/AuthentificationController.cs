
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

        //private string GetRoleByName(string roleName)
        //{
        //    roleName = roleName.ToUpper();
        //    List<IdentityRole> roles = roleManager.Roles.Where(y => y.NormalizedName.Equals(roleName)).ToList();

        //    return roles[0].Id;
        //}


        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
               var list = _repository.GetAll(null, null);
            return Ok(list); 
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
