
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MiniProjectBack.ModelsAuth;
using PortalVioo.Interface;
using PortalVioo.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PortalVioo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController(IEmailSender emailSender,IRepositoryGenericApp<ApplicationUser> repository, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager = userManager;
        private readonly RoleManager<IdentityRole> roleManager = roleManager;
        private readonly IConfiguration _configuration = configuration;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IRepositoryGenericApp<ApplicationUser> _repository = repository;


        [HttpGet]
        [Route("GetRoles")]
        public IActionResult GetRoles()
        {

            List<IdentityRole> roles = roleManager.Roles.ToList();

            return Ok(roles);
        }

        [HttpPost]
        [Route("AddRole")]
        public async Task<IActionResult> AddRole(string role)
        {

            bool x = await roleManager.RoleExistsAsync(role);
            if (!x)
            {
                // first we create Admin rool    
                var roles = new IdentityRole();
                roles.Name = role;
               // roles.NormalizedName =NormalizedName;
                await roleManager.CreateAsync(roles);
                return Ok( new { role });
            }
            else return BadRequest();
        }

        [HttpPost]
        [Route("CHECKROLE")]
        public async Task<IActionResult> CHECKROLE(string role)
        {
            bool exist = true;
            int i = 0;
            var listUser = _repository.GetAll(null, null);
            while (exist = true && i < listUser.Count)
            {
                var roleUser = await userManager.GetRolesAsync(listUser[i]);
                IdentityRole rolesCheck = roleManager.Roles.Where(x => x.Name.ToLower() == roleUser[0].ToLower()).FirstOrDefault();
                if (rolesCheck.Name.ToLower()== role.ToLower()) { exist = true; }
                else { i++; }
            }
            if (exist) { return BadRequest("role exist in a user"); }
            else { return Ok("great"); }
        }



        [HttpPost]
        [Route("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string role)
        {
           

            var roles = await roleManager.FindByNameAsync(role);
            var result = await roleManager.DeleteAsync(roles);
            if (result.Succeeded)
            { 
                return Ok(new { role });
            }
            else return BadRequest();
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
                Phone = user.PhoneNumber,
                ImgPath = user.ImgPath,
                Nom = user.NomUser,
                prenom = user.PrenomUser
            });
        }

        [HttpPost]
        [Route("ChangeRole")]
        public async Task<IActionResult> ChangeRole([FromBody] ChangeRoleModel cr)
        {
            var user = await userManager.FindByNameAsync(cr.username);
            var role = await userManager.GetRolesAsync(user);
            IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == role[0].ToLower()).FirstOrDefault();
            string roletoadd = roleManager.Roles.Where(x => x.Name.ToLower() == cr.role.ToLower()).FirstOrDefault().NormalizedName;
            await userManager.RemoveFromRoleAsync(user, roles.NormalizedName);
            var result = await userManager.AddToRoleAsync(user, roletoadd);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            { return Ok( cr.role); }

        }



        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel ch)
        {
            var user = await userManager.FindByNameAsync(ch.username);
            IdentityResult result2;
            foreach (IPasswordValidator<ApplicationUser> passwordValidator in userManager.PasswordValidators)
            {
                result2 = await passwordValidator.ValidateAsync(userManager, user, ch.password);

                if (!result2.Succeeded)
                {
                    return BadRequest(result2.Errors);
                }
            }
            ch.token = await userManager.GeneratePasswordResetTokenAsync(user);
            var resetPassResult = await userManager.ResetPasswordAsync(user, ch.token, ch.password);
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                _emailSender.SendEmail(user.Email, "Changement Mot de Passe",ch.password);
                return Ok(new { ch.password });
            }
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
                usrsRole.RoleNormalizedName = roles.NormalizedName;
                usrsRole.Username = user.UserName;
                usrsRole.Email = user.Email;
                usrsRole.Role = roles.Name;
                usrsRole.nom = user.NomUser;
                usrsRole.prenom = user.PrenomUser;
                usrsRole.imagePath = user.ImgPath;
                listUserWithRole.Add(usrsRole);

            }
            return Ok(listUserWithRole);
        }

        [HttpGet]
        [Route("GetGestionnaire")]
        public async Task<IActionResult> GetGestionnaire()
        {
            List<getuserwthrole> listUserWithRole = new List<getuserwthrole>();
            List<getuserwthrole> filteredList = new List<getuserwthrole>();
            var listUser = _repository.GetAll(null, null);
            foreach (var user in listUser)
            {
                var role = await userManager.GetRolesAsync(user);
                IdentityRole roles = roleManager.Roles.Where(x => x.Name.ToLower() == role[0].ToLower()).FirstOrDefault();
                getuserwthrole usrsRole = new getuserwthrole();
              
                usrsRole.UserId = user.Id;
                usrsRole.RoleId = roles.Id;
                usrsRole.RoleNormalizedName = roles.NormalizedName;
                usrsRole.Username = user.UserName;
                usrsRole.Email = user.Email;
                usrsRole.Role = roles.Name;
                usrsRole.nom = user.NomUser;
                usrsRole.prenom = user.PrenomUser;
                usrsRole.imagePath = user.ImgPath;
                listUserWithRole.Add(usrsRole);
                filteredList = listUserWithRole.Where(x => x.RoleNormalizedName== "GESTIONNAIRE").ToList();


            }
            return Ok(filteredList);
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
                var tk = new JwtSecurityTokenHandler().WriteToken(token);

                //Response.Cookies.Append("X-Access-Token", tk, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict , Expires= DateTime.Now.AddMinutes(50)});


                return Ok(new
                {
                    token = tk,
                    expiration = token.ValidTo,
                    role = userRoles[0],
                    nom = user.NomUser ,
                    prenom = user.PrenomUser ,
                    img =user.ImgPath,
                    username = user.UserName
                });
            }
            return Unauthorized();
        }





        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model, string Role)
        {
            IdentityResult result;
            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                NomUser = model.NomUser,
                PrenomUser = model.PrenomUser,
                ImgPath = model.ImagePath,
                PhoneNumber = model.phone


            };
            foreach (IPasswordValidator<ApplicationUser> passwordValidator in userManager.PasswordValidators)
            {
                result = await passwordValidator.ValidateAsync(userManager, user, model.Password);

                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
            }
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user2 = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                NomUser=model.NomUser,
                PrenomUser=model.PrenomUser,
                ImgPath=model.ImagePath,
                PhoneNumber = model.phone
                
                
            };
            var result2 = await userManager.CreateAsync(user, model.Password);
            await userManager.AddToRoleAsync(user, Role);
            if (await roleManager.RoleExistsAsync(Role))
            {
                await userManager.AddToRoleAsync(user, Role);
            }
            if (!result2.Succeeded)
            {               
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message ="error" });   
            }
              
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

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}
