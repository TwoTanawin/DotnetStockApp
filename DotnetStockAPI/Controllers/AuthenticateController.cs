using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotnetStockAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace DotnetStockAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // create object manage users
        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _configuration;

        //Constructor for initial ApplicationDbContext
        public AuthenticateController(
            ApplicationDbContext context,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }

        // test connection DB
        [HttpGet("testConnectDB")]
        public void TestConnection()
        {
            if (_context.Database.CanConnect())
            {
                Response.WriteAsync("Connected");
            }
            else
            {
                Response.WriteAsync("Not Connected");
            }
        }

        // Register for User
        // Post api/authenticate/register-user
        [HttpPost]
        [Route("register-user")]
        public async Task<ActionResult> RegisterUser([FromBody] RegisterModel model)
        {
            // เช็คว่า username ซ้ำหรือไม่
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User already exists!"
                    }
                );
            }

            // เช็คว่า email ซ้ำหรือไม่
            userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "Email already exists!"
                    }
                );
            }

            // สร้าง User
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            // สร้าง User ในระบบ
            var result = await _userManager.CreateAsync(user, model.Password);

            // ถ้าสร้างไม่สำเร็จ
            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    }
                );
            }

            // กำหนด Roles Admin, Manager, User
            if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Manager));
            }

            if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
                await _userManager.AddToRoleAsync(user, UserRolesModel.User);
            }

            return Ok(new ResponseModel
            {
                Status = "Success",
                Message = "User registered successfully"
            });
        }


        // Register for Manager
        // Post api/authenticate/register-manger
        [HttpPost]
        [Route("register-manger")]
        public async Task<ActionResult> RegisterManager([FromBody] RegisterModel model)
        {
            // เช็คว่า username ซ้ำหรือไม่
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User already exists!"
                    }
                );
            }

            // เช็คว่า email ซ้ำหรือไม่
            userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "Email already exists!"
                    }
                );
            }

            // สร้าง User
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            // สร้าง User ในระบบ
            var result = await _userManager.CreateAsync(user, model.Password);

            // ถ้าสร้างไม่สำเร็จ
            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    }
                );
            }

            // กำหนด Roles Admin, Manager, User
            if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
            }

            if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
            }

            if (await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
            {
                await _userManager.AddToRoleAsync(user, UserRolesModel.Manager);
            }

            return Ok(new ResponseModel
            {
                Status = "Success",
                Message = "User registered successfully"
            });
        }

        // Register for Admin
        // Post api/authenticate/register-manger
        [HttpPost]
        [Route("register-admin")]
        public async Task<ActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            // เช็คว่า username ซ้ำหรือไม่
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User already exists!"
                    }
                );
            }

            // เช็คว่า email ซ้ำหรือไม่
            userExists = await _userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "Email already exists!"
                    }
                );
            }

            // สร้าง User
            IdentityUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };

            // สร้าง User ในระบบ
            var result = await _userManager.CreateAsync(user, model.Password);

            // ถ้าสร้างไม่สำเร็จ
            if (!result.Succeeded)
            {
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new ResponseModel
                    {
                        Status = "Error",
                        Message = "User creation failed! Please check user details and try again."
                    }
                );
            }

            // กำหนด Roles Admin, Manager, User
            if (!await _roleManager.RoleExistsAsync(UserRolesModel.Admin))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.Admin));
                await _userManager.AddToRoleAsync(user, UserRolesModel.Admin);
            }

            if (!await _roleManager.RoleExistsAsync(UserRolesModel.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(UserRolesModel.User));
            }

            if (!await _roleManager.RoleExistsAsync(UserRolesModel.Manager))
            {
                await _userManager.AddToRoleAsync(user, UserRolesModel.Manager);
            }

            return Ok(new ResponseModel
            {
                Status = "Success",
                Message = "User registered successfully"
            });
        }

        // Login for User
        // Post api/authenticate/login-user
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginModel model)
        {

            var user = await _userManager.FindByNameAsync(model.Username!);

            // ถ้า login สำเร็จ
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }

            // ถ้า login ไม่สำเร็จ
            return Unauthorized();
        }

        // ฟังก์ชันสร้าง Token
        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]!));

            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"); // Windows time zone ID

            // Get the current time in Bangkok time zone
            var currentTime = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: currentTime.AddHours(24),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return token;
        }
        // Logout
        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name;
            if (userName != null)
            {
                var user = await _userManager.FindByNameAsync(userName);
                if (user != null)
                {
                    await _userManager.UpdateSecurityStampAsync(user);
                    return Ok(new ResponseModel { Status = "Success", Message = "User logged out!" });
                }
            }
            return Ok();
        }

        // Refresh Token
        [HttpPost]
        [Route("refresh-token")]
        public IActionResult RefreshToken([FromBody] RefreshTokenModel model)
        {
            var authHeader = Request.Headers["Authorization"];
            if (authHeader.ToString().StartsWith("Bearer"))
            {
                var token = authHeader.ToString().Substring(7);
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]!);

                try
                {
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = false,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var user = new
                    {
                        Name = jwtToken.Claims.First(x => x.Type == "unique_name").Value,
                        Role = jwtToken.Claims.First(x => x.Type == ClaimTypes.Role).Value
                    };

                    var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };

                    var newToken = GetToken(authClaims);
                    return Ok(new
                    {
                        token = new JwtSecurityTokenHandler().WriteToken(newToken),
                        expiration = newToken.ValidTo
                    });
                }
                catch
                {
                    return Unauthorized();
                }
            }

            return Unauthorized();
        }

        public class RefreshTokenModel
        {
            public required string Token { get; set; }
        }
    }
}