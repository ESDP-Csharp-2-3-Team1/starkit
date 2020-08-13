using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
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
        public UserManager<User> _userManager { get; set; }
        public RoleManager<IdentityRole> _roleManager { get; set; }
        public SignInManager<User> _signInManager { get; set; }
        public StarkitContext _db { get; set; }
        public IHostEnvironment _environment { get; set; }
        
        

        public AccountController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, SignInManager<User> signInManager, StarkitContext db, IHostEnvironment environment, IRecaptchaService recaptcha)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _db = db;
            _environment = environment;
            _recaptcha = recaptcha;
        }

        // GET

        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Users");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(Login model)
        {
            if (ModelState.IsValid)
            {
                User user = await _db.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user != null)
                {
                    if (user.AccessFailedCount >= 3)
                    {
                        var captchaResponse = await _recaptcha.Validate(Request.Form);
                        if (!captchaResponse.Success)
                        {
                            ModelState.AddModelError("reCaptchaError", 
                                "Ошибка проверки reCAPTCHA. Попробуйте еще раз.");
                            return View(model);
                        }
                    }
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
                    if (result.Succeeded)
                    {
                        if (user.AccessFailedCount != 0)
                        {
                            user.AccessFailedCount = 0;
                            await _userManager.UpdateAsync(user);
                        }
                        return RedirectToAction("Index", "Users");
                    }
                    user.AccessFailedCount += 1;
                    await _userManager.UpdateAsync(user);
                    ModelState.AddModelError("", "Неверная попытка входа в систему");
                    return View();
                }
                ModelState.AddModelError("","Неверная попытка входа в систему");
            }
            return View(model);
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index","Users");
            return View();
        }
        
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
                User newUser = new User
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

                model.PostalAddress.UserId = newUser.Id;
                model.LegalAddress.UserId = newUser.Id;

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, "restaurateur");
                    await _db.LegalAddresses.AddAsync(model.LegalAddress);
                    await _db.PostalAddresses.AddAsync(model.PostalAddress);
                    await _db.SaveChangesAsync();
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
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

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
    }
}