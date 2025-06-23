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
using System.Security.Cryptography;
using Azure;
using Structure;

namespace DemoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenRepository _tokenService;
        private IPasswordHasher<ApplicationUser> passwordHasher;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        public AccountController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ITokenRepository TokenService, IUnitOfWork unitOfWork, IMapper mapper, IPasswordHasher<ApplicationUser> passwordHash)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            passwordHasher = passwordHash;
            _mapper = mapper;
            _tokenService = TokenService;
            _unitOfWork = unitOfWork;
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
                ApplicationUser user = await userManager.FindByEmailAsync(UserDto.UserName);
                //ApplicationUser user = await userManager.FindByNameAsync(UserDto.UserName);
                if (user != null)
                {
                    bool found = await userManager.CheckPasswordAsync(user, UserDto.Password);
                    if (found)
                    {
                        var token = await _tokenService.GenAccessToken(user);
                        var refreshToken = await _tokenService.CreateRefreshToken(user);
                        return Ok(new ResponseDto
                        {
                            token = token,
                            refreshToken = refreshToken.Token,
                            refreshTokenExpiration = UserDto.RememberMe ? refreshToken.Expiry : DateTime.UtcNow.AddDays(1),
                        });
                    }
                }
            }
            return Unauthorized();
        }

        [HttpPost("refresh-token")]
        //[Authorize]
        public async Task<IActionResult> RefreshToken([FromBody] string RefreshToken)
        {
            if (RefreshToken is null)
                return BadRequest("Request body is null");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var refreshToken = await _tokenService.GetRefreshToken(RefreshToken);
            if (refreshToken is null)
                return Unauthorized("Invalid or expired refresh token");

            var response = new ResponseDto()
            {
                token = refreshToken.token,
                refreshToken = refreshToken.refreshToken,
                refreshTokenExpiration = refreshToken.refreshTokenExpiration,
            };

            return Ok(response);
        }


        [HttpPost("EditUserData")]
        //[Authorize]
        public async Task<IActionResult> EditUserData(EditUserDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(UserDto.Id);
                if (user != null)
                {
                    user.Name = UserDto.Name;
                    user.UserName = UserDto.UserName;
                    user.Email = UserDto.Email;
                    user.PhoneNumber = UserDto.Phone;
                    user.JobId = UserDto.JobId;
                    //user.PasswordHash = passwordHasher.HashPassword(user, UserDto.Password);


                    //var roles = await userManager.GetRolesAsync(user);
                    //await userManager.RemoveFromRolesAsync(user, roles);
                    await RemoveAllRoles(user.Id);
                    var role = await roleManager.FindByIdAsync(UserDto.RoleId);
                    await userManager.AddToRoleAsync(user, role.Name);

                    //await userManager.RemovePasswordAsync(user);
                    //await userManager.AddPasswordAsync(user, UserDto.Password);
                    //await userManager.ChangePasswordAsync(user, UserDto.Oldpassword, UserDto.Password);

                    IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok(user);
                    else
                        return BadRequest(result.Errors.FirstOrDefault());
                }
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost("EditUserPassword")]
        //[Authorize]
        public async Task<IActionResult> EditUserPassword(EditUserPasswordDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(UserDto.Id);
                if (user != null)
                {
                    //await userManager.RemovePasswordAsync(user);
                    //await userManager.AddPasswordAsync(user, UserDto.Password);

                    //await userManager.ChangePasswordAsync(user, UserDto.Oldpassword, UserDto.Password);
                    var result = await userManager.ChangePasswordAsync(user, UserDto.Oldpassword, UserDto.Password);
                    //IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok(user);
                    else
                        return BadRequest(result.Errors.FirstOrDefault());
                }
            }
            return BadRequest(ModelState);
        }
        
        [HttpPost("EditUserPasswordAdmin")]
        //[Authorize]
        public async Task<IActionResult> EditUserPasswordAdmin(EditUserPasswordDto UserDto)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await userManager.FindByIdAsync(UserDto.Id);
                if (user != null)
                {
                    await userManager.RemovePasswordAsync(user);
                    var result = await userManager.AddPasswordAsync(user, UserDto.Password);

                    //var result = await userManager.ChangePasswordAsync(user, UserDto.Oldpassword, UserDto.Password);
                    //IdentityResult result = await userManager.UpdateAsync(user);
                    if (result.Succeeded)
                        return Ok(user);
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
                var roles = await userManager.GetRolesAsync(user);
                string roleName = roles.Count == 0 ? "" : roles[0];
                userToReturn.RoleName = roleName;
                userToReturn.RoleId = (roles.Count == 0 ? "" : await GetRoleId(roleName));
                return Ok(userToReturn);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        #region RefreshTokenMethods
        //private string GenerateRefreshToken()
        //{
        //    var randomNumber = new byte[32];
        //    using (var rng = RandomNumberGenerator.Create())
        //    {
        //        rng.GetBytes(randomNumber);
        //        return Convert.ToBase64String(randomNumber);
        //    }
        //}

        //public async Task<(string Token, DateTime Expiry)> CreateRefreshToken(ApplicationUser user)
        //{
        //    if(user == null)
        //    {
        //        throw new ArgumentNullException(nameof(user));
        //    }

        //    var token = GenerateRefreshToken();

        //    var refreshToken = new RefreshToken()
        //    {
        //        Token = token,
        //        UserId = user.Id,
        //        Expires = DateTime.UtcNow.AddDays(7),
        //        User = user
        //    };
        //    try
        //    {
        //        _unitOfWork.RefreshToken.Add(refreshToken);
        //        await _unitOfWork.Complete();

        //        return(token, refreshToken.Expires);
        //    }
        //    catch(Exception ex)
        //    {
        //        throw new Exception("Error creating refresh token", ex);
        //    }
        //}

        //public async Task<ResponseDto?> GetRefreshToken(string request)
        //{
        //   if(string.IsNullOrEmpty(request)) throw new ArgumentNullException(nameof(request));
        //   RefreshToken? refreshToken = await _unitOfWork.RefreshToken
        //        .GetFirstorDefault(c => c.Token == request, IncludeWord:"User");

        //    if (refreshToken?.Expires < DateTime.UtcNow || refreshToken == null)
        //    {
        //        throw new ApplicationException("refresh token expired");
        //    }

        //    string accessToken = await GenerateAccessToken(refreshToken.User);
        //    refreshToken.Token = GenerateRefreshToken();
        //    refreshToken.Expires = DateTime.UtcNow.AddDays(7);
        //    await _unitOfWork.Complete();

        //    return new ResponseDto()
        //    {
        //        token = accessToken,
        //        refreshToken = refreshToken.Token,
        //        refreshTokenExpiration = refreshToken.Expires,
        //    };

        //}

        //public async Task<string> GenerateAccessToken(ApplicationUser user)
        //{
        //    var userRoles = await userManager.GetRolesAsync(user);
        //    var claims = new List<Claim>

        //                {
        //                     new Claim(ClaimTypes.Name, user.UserName),
        //                     new Claim(ClaimTypes.NameIdentifier, user.Id),
        //                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //                     //new Claim(ClaimTypes.Role, userRoles.FirstOrDefault()),
        //                };
        //    foreach (var role in userRoles)
        //    {
        //        claims.Add(new Claim(ClaimTypes.Role, role));
        //    }

        //    SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"]));
        //    SigningCredentials signingCred = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    //Token
        //    JwtSecurityToken myToken = new JwtSecurityToken(
        //        issuer: config["JWT:ValidIssuer"],
        //        audience: config["JWT:ValidVudience"],
        //        claims: claims,
        //        expires: DateTime.Now.AddHours(2),
        //        signingCredentials: signingCred
        //    );

        //    return new JwtSecurityTokenHandler().WriteToken(myToken);
        //}
        #endregion


        [HttpGet("GetEmpComms")]
        public async Task<IActionResult> GetEmpComms(string empId, DateTime fromDate, DateTime toDate)
        {
            var list = _unitOfWork.Setting.GetEmpComms(empId, fromDate, toDate);
            if (list.Count() > 0)
                return Ok(list);
            else
                return NotFound(new { message = "لا يوجد بيانات لعرضها", status = 404 });
        }



        #region MyRegion
        [HttpPost("remove-all-roles/{userId}")]
        public async Task<IActionResult> RemoveAllRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null) return NotFound($"User with ID {userId} not found.");

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);
            return result.Succeeded ? Ok($"All roles removed from user {userId}.") : BadRequest(result.Errors);
        }

        [HttpPost("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                return BadRequest("Role name is required.");
            }

            var roleExists = await roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
                return BadRequest($"Role '{roleName}' already exists.");
            }

            var role = new IdentityRole(roleName);
            var result = await roleManager.CreateAsync(role);
            return result.Succeeded ? Ok($"Role '{roleName}' created successfully.") : BadRequest(result.Errors);
        }
        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound($"Role '{roleName}' not found.");
            }

            var result = await roleManager.DeleteAsync(role);
            return result.Succeeded ? Ok($"Role '{roleName}' deleted successfully.") : BadRequest(result.Errors);
        }
        [HttpGet("GetRoleId")]
        public async Task<string> GetRoleId(string name)
        {
            var role = await roleManager.FindByNameAsync(name);
            return (role.Id);
        }
        [HttpGet("GetAllRoles")]
        public IActionResult GetAllRoles()
        {
            var roles = roleManager.Roles;
            return Ok(roles);
        }
        // GET: api/roles/user/{userId}
        [HttpGet("GetUserRoles/{userId}")]
        public async Task<IActionResult> GetUserRoles(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }

            var roles = await userManager.GetRolesAsync(user);
            return Ok(roles);
        }
        #endregion
    }
}
