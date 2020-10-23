using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Starkit.Models;
using Starkit.Models.Data;
using Starkit.Services;
using Starkit.ViewModels;

namespace Starkit.Controllers
{
    public class AccountController : Controller
    {
        private readonly IRecaptchaService _recaptcha;
        private UserManager<User> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private SignInManager<User> _signInManager;
        private StarkitContext _db;
        private IHostEnvironment _environment;
        
        
        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, StarkitContext db, IHostEnvironment environment, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
            _environment = environment;
            _recaptcha = recaptcha;
        }
        
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Users");
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            int fails = Convert.ToInt32(HttpContext.Session.GetInt32("fails"));
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    if (user.AccessFailedCount >= 3 || fails >= 3)
                    {
                        var captchaResponse =  Captcha.ValidateCaptchaCode(model.CaptchaCode, HttpContext);
                        if (!captchaResponse)
                        {
                            ModelState.AddModelError("", 
                                "Неверная попытка входа в систему");
                            return View(model);
                        }
                    }
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (user.AccessFailedCount != 0)
                        {
                            user.AccessFailedCount = 0;
                            fails = 0;
                            await _userManager.UpdateAsync(user);
                        }
                        return RedirectToAction("Initial", "Account");
                    }
                    user.AccessFailedCount += 1;
                    fails += 1;
                    HttpContext.Session.SetInt32("fails", fails);
                    await _userManager.UpdateAsync(user);
                    ModelState.AddModelError("", "Неверная попытка входа в систему");
                    return View();
                }
                ModelState.AddModelError("","Неверная попытка входа в систему");
            }
            fails += 1;
            HttpContext.Session.SetInt32("fails", fails);
            return View(model);
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Initial","Account");
            return View();
        }
        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Register(Register model)
        {
            if(ModelState.IsValid)
            {
                var captchaResponse = await _recaptcha.Validate(Request.Form);
                if (!captchaResponse.Success)
                {
                    ModelState.AddModelError("reCaptchaError", 
                        "Ошибка проверки reCAPTCHA. Попробуйте еще раз.");
                    return View(model);
                }

                User newUser = CreateNewUser(model);
                model.PostalAddress.UserId = newUser.Id;
                model.LegalAddress.UserId = newUser.Id;

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Convert.ToString(Roles.Registrant));
                    await _db.LegalAddresses.AddAsync(model.LegalAddress);
                    await _db.PostalAddresses.AddAsync(model.PostalAddress);
                    await _db.SaveChangesAsync();
                    // await SendConfirmationEmailAsync(newUser.Email);
                    if (User.IsInRole("SuperAdmin"))
                        return RedirectToAction("Index", "SuperAdmin");
                    await _signInManager.SignInAsync(newUser, false);
                    return RedirectToAction("Index", "Users");
                }

                foreach (var error in result.Errors)
                    ModelState.AddModelError(String.Empty, error.Description);
            }
            return View(model);
        }

        

        [Authorize]
        public async Task<IActionResult> Confirm(string email)
        {
            if (email != null)
            {
                User user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    if (user.EmailConfirmed)
                        return View();
                    user.EmailConfirmed = true;
                    _db.Users.Update(user);
                    await _db.SaveChangesAsync();
                    return View();
                }
            }
            return NotFound();
        }

        [NonAction]
        private async Task SendConfirmationEmailAsync(string email)
        {
            string filePath = Path.Combine(_environment.ContentRootPath, "wwwroot/HTML_Template/ConfirmationEmail.txt");
            string message = await System.IO.File.ReadAllTextAsync(filePath);
            MailService emailServices = new MailService();
            await emailServices.SendEmailAsync(
                email,
                "Приветственное письмо!",
                message
            );
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            if (User.IsInRole("SuperAdmin"))
            {
                User admin = await _db.Users.FirstOrDefaultAsync(u => u.Id == _userManager.GetUserId(User));
                admin.IdOfTheSelectedRestaurateur = null;
                _db.Users.Update(admin);
                await _db.SaveChangesAsync();
            }
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string id, string oldPassword, string newPassword)
        {
            User user = _userManager.FindByIdAsync(id).Result;
                
                if (user != null)
                {
                    EditUserViewModel model = new EditUserViewModel()
                    {
                        Id = user.Id,
                        OldPassword = oldPassword,
                        NewPassword = newPassword
                    };
                    var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
                    if (result.Succeeded)
                    {
                        return Ok("Пароль успешно изменен");
                    }
                    return PartialView("~/Views/Users/PartialViews/EditPasswordPartialView.cshtml", model);
                }

                return NotFound();
        }

        public async Task<IActionResult> FindUserByEmail(string email)
        {
            if (email != null)
            {
                User user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                    return Json(user.AccessFailedCount);
                var fails = Convert.ToInt32(HttpContext.Session.GetInt32("fails"));
                return Json(fails);
            }

            return NoContent();

        }
        
        
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
 
            var user = await _userManager.FindByEmailAsync(model.Email);
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callback = Url.Action("ResetPassword", "Account", new {token, email = user.Email}, Request.Scheme);
            
            MailService mailService = new MailService();
            await mailService.SendEmailAsync(model.Email, "Восстановление пароля", callback);
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordViewModel { Token = token, Email = email };
            return View(model);
        }
 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                RedirectToAction("ResetPasswordConfirmation");
 
            var resetPassResult = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if(!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
 
                return View();
            }
 
            return RedirectToAction("ResetPasswordConfirmation");
        }
 
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return View("Error");
 
            var result = await _userManager.ConfirmEmailAsync(user, token);
            return View(result.Succeeded ? "ConfirmEmail" : "Login");
        }
 
        [HttpGet]
        public IActionResult SuccessRegistration()
        {
            return View();
        }
        
        [NonAction]
        private static User CreateNewUser(Register model)
        {
            User user = new User
            {
                UserName =  model.Email,
                Name = model.Name,
                SurName = model.SurName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                CityPhone = model.CityPhone,
                CompanyName = model.CompanyName,
                IIN = model.IIN,
                IsTermsAccepted = model.IsTermsAccepted
            };
            return user;
        }
        public IActionResult Initial()
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");
            if (User.IsInRole(Convert.ToString(Roles.Registrant)))
                return RedirectToAction("Index", "Users");
            if(User.IsInRole(Convert.ToString(Roles.ContentManager)))
                return RedirectToAction("Index", "Dishes");
            return RedirectToAction("Index", "SuperAdmin");
        }
        
        [Route("get-captcha-image")]
        public IActionResult GetCaptchaImage()
        {
            int width = 100;
            int height = 36;
            var captchaCode = Captcha.GenerateCaptchaCode();
            var result = Captcha.GenerateCaptchaImage(width, height, captchaCode);
            HttpContext.Session.SetString("CaptchaCode", result.CaptchaCode);
            Stream s = new MemoryStream(result.CaptchaByteData);
            return new FileStreamResult(s, "image/png");
        }

    }
}