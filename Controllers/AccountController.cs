using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using AuthService.Models;
using AuthService.Models.AccountViewModels;
using AuthService.Services;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using AuthService.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using AuthService.Models.ProfileViewModels;

//Dani Pereira

namespace AuthService.Controllers
{
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _context;
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender,
            ILoggerFactory loggerFactory,
            ApplicationDbContext context,
            IStringLocalizer<AccountController> localizer,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            _logger = loggerFactory.CreateLogger<AccountController>();
            _context = context;
            _localizer = localizer;
            _configuration = configuration;
        }


       
        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Login([FromBody] LoginViewModel model)
        {
             if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation(1, "User logged in.");
                    var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                    return await GenerateJwtToken(model.Email, appUser);
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning(2, "User account locked out.");
                    throw new ApplicationException("ACCOUNT_IS_LOCKED_OUT");
                }
            }

             _logger.LogWarning(3, "Invalid login attempt.");
            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");

        }



        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<object> Register([FromBody] RegisterViewModel model)
        {

             if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };


                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    var result = await _userManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation(1, "User created a new account with password.");
                        var identityRole = new IdentityUserRole<string>();

                        identityRole.RoleId = "0480424a-2820-4c3a-a604-de187d0bb0f2"; //TODO: REMOVE BY FIND
                        identityRole.UserId = user.Id;

                        //Create role
                        result = await _userManager.AddToRoleAsync(user, "SITEUSER");

                        if (result.Succeeded)
                        {
                            _logger.LogInformation(2, "User role created.");
                            //Create employer profile
                            var profile = new ProfileViewModel(){
                                Id = Guid.NewGuid(),
                                Name =String.Empty,
                                UserId = user.Id
                            };

                            _context.Add(profile);
                            var rowsAffected  = await _context.SaveChangesAsync();
                            if (rowsAffected > 0)
                            {
                                _logger.LogInformation(3, "User profile created");
                                // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=532713
                                // Send an email with this link
                                //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                                //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                                //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                                await _signInManager.SignInAsync(user, isPersistent: false);
                                _logger.LogInformation(4, "User logged in.");

                                transaction.Commit();
                                return await GenerateJwtToken(model.Email, user);
                                
                             }
                        }
                    }
                    transaction.Rollback();
                }
            }
            _logger.LogInformation(3, "Unknown error");
            throw new ApplicationException("UNKNOWN_ERROR");
        }



        private async Task<object> GenerateJwtToken(string email, ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                _configuration["JwtIssuer"],
                _configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
