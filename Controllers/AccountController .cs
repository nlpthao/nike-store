using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using NikeStyle.Models;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Packaging;

namespace NikeStyle.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailSender _emailSender;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            
        }

        //LOGIN
        [HttpGet]
        public async Task<IActionResult> Login(string email = null)
        {
            var model = new LoginViewModel
            {
                Email = email,
                ReturnUrl = "/",
            };
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Attempt to sign in the user
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/");
            }
            else if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Your account is locked out.");
            }
            else if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Login is not allowed for this user.");
            }
            else if (result.RequiresTwoFactor)
            {
                ModelState.AddModelError(string.Empty, "Two-factor authentication is required.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            // If login fails, add an error and return the view
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }
        //REGISTER
        [HttpGet]
        public IActionResult Register(string email = null)
        {
            var model = new RegisterViewModel
            {
                Email = email
            };
            TempData.Keep("VerificationCode");
            TempData.Remove("UserEmail");
            return View(model);
        }
        
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate the code entered by the user
            if (TempData["VerificationCode"]?.ToString() != model.Code || TempData["UserEmail"]?.ToString() != model.Email)
            {
                ModelState.AddModelError(string.Empty,"Invalid verification code.");
            }

            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }
        // JOIN US
        [HttpGet]
        public IActionResult JoinUs()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> JoinUs (JoinUsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Check if a user with this email exists
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user !=null)
            {
                return RedirectToAction("Login","Account",new {email = model.Email});
            }
            else
            {
                var verificationCode = GenerateVerificationCode();
                TempData["VerificationCode"] = verificationCode;
                TempData["UserEmail"] = model.Email;

                // Send the verification code to email
                await _emailSender.SendEmailAsync(model.Email,"Your Verification code",$"Your verification code is:{verificationCode}");

                // Redirect to register email and pass the email
                return RedirectToAction("Register","Account", new {email = model.Email});
            }
        }
        private string GenerateVerificationCode()
        {
            var random = new Random();
            return random.Next(100000,999999).ToString(); //Generate 6-digit code
        }

        //LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home"); //Redirect home page after logout
        }

        // FORGOT PASSWORD
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword (ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {return View(model);}

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetlink = Url.Action("ResetPassword","Account",new {token,email=model.Email}, Request.Scheme);

            // Send email
            await _emailSender.SendEmailAsync(model.Email,"Reset Password", $"Please reset your password by clicking here: <a href='{resetlink}'>link</a>");
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        //FORGOT CONFIRMATION
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //RESET PASSWORD
        public IActionResult ResetPassword(string token = null, string email = null)
        {
            if (token == null || email == null)
            {
                ModelState.AddModelError(string.Empty,"Invalid password reset token");
            }

            var model = new ResetPasswordViewModel {Token = token, Email = email};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword (ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid){
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }

            var result = await _userManager.ResetPasswordAsync(user,model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View();
        }

        //RESET CONFIRMATION
        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResendEmailConfirmation(ResendEmailConfirmationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return RedirectToAction("ForgotPasswordConfirmation");
            }
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action("ConfirmEmail", "Account", new { token, email = model.Email }, Request.Scheme);

            // Send email
            await _emailSender.SendEmailAsync(model.Email, "Confirm your email", $"Please confirm your email by clicking here: <a href='{confirmationLink}'>link</a>");

            return RedirectToAction("ForgotPasswordConfirmation");
        }
    }
}