using Entities.DTO;
using Entities.Models;
using DataAccess.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Entities.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Utilities;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration config;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IMapper _mapper;
        public AccountController(UserManager<ApplicationUser> userManager,IConfiguration config, IMapper mapper, IPasswordHasher<ApplicationUser> passwordHash)
        {
            this.userManager = userManager;
            this.config = config;
            passwordHasher = passwordHash;
            _mapper = mapper;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Registration(RegisterUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = UserDto.UserName;
                user.Email = UserDto.Email;
                user.PhoneNumber = UserDto.Phone;
                user.EmailConfirmed = true;
                user.PhoneNumberConfirmed = true;
                user.LockoutEnabled = false;
                user.JobId = 5;

                IdentityResult result = await userManager.CreateAsync(user, UserDto.Password);
                if (result.Succeeded)
                {
                  var roleResult = await userManager.AddToRoleAsync(user, "Employee");
                    if (roleResult.Succeeded)
                    {
                        return Ok("Account Created Successfully");
                    }
                }
                return BadRequest(result.Errors.FirstOrDefault());
            }            
            return BadRequest(ModelState);
        }
        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByNameAsync(UserDto.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, UserDto.Password);
                    if (found)
                    {
                        //var claims = new List<Claim>();
                        ////Claims
                        // claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                        // claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                        // claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                        ////get role
                        //var roles = await userManager.GetRolesAsync(user);
                        //foreach (var Roleitem in roles)
                        //{
                        //    claims.Add(new Claim(ClaimTypes.Role, Roleitem));
                        //}
                        var userRoles = await userManager.GetRolesAsync(user);
                        var claims = new List<Claim>

                        {
                             new Claim(ClaimTypes.Name, user.UserName),
                             new Claim(ClaimTypes.NameIdentifier, user.Id),
                             new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                             //new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()),
                        };
                        foreach (var role in userRoles)
                        {
                            claims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
                        SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                        //Token
                        JwtSecurityToken myToken = new JwtSecurityToken(
                            issuer: config["JWT:ValidIssuer"],
                            audience: config["JWT:ValidVudience"],
                            claims: claims,
                            expires: DateTime.Now.AddHours(2),
                            signingCredentials: signingCred
                            );
                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(myToken)
                        });
                    }
                }
            }
            return Unauthorized();
        }

       
        [HttpPost("EditUser")]
        [Authorize]
        public async Task<IActionResult> EditUser(EditUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(UserDto.Id);
                if (user != null)
                {
                    user.UserName = UserDto.UserName;
                    user.Email = UserDto.Email;
                    user.PhoneNumber = UserDto.Phone;
                    user.JobId = UserDto.JobId;
                    //user.PasswordHash = passwordHasher.HashPassword(user, UserDto.Password);
                    await userManager.RemovePasswordAsync(user);
                    await userManager.AddPasswordAsync(user, UserDto.Password);

                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok("User Updated Successfully");
                    else
                        return BadRequest(result.Errors.FirstOrDefault());
                }
            }
            return BadRequest(ModelState);
        }

        
        [HttpGet("GetAllUsers")]
        [Authorize(Roles = SD.AdminRole)]
        //[Authorize(AuthenticationSchemes = "Bearer", Roles = nameof(SD.AdminRole))]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await userManager.Users.Include(z => z.Job).ToListAsync();
            var usersToReturn = _mapper.Map<IEnumerable<DisplayUserDto>>(users);
            return Ok(usersToReturn);
        }

        [HttpGet("GetUserById")]
        [Authorize]
        public async Task<IActionResult> GetUserById(string Id)
        {
            try
            {
              
                //var user = await userManager.FindByIdAsync(Id);
                var user = await userManager.Users.Include(z => z.Job).FirstOrDefaultAsync(u => u.Id == Id);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                var userToReturn = _mapper.Map<DisplayUserDto>(user);
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


    }
}
