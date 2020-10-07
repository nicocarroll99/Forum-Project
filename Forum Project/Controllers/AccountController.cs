using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Forum_Project.Models;
using Forum_Project.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum_Project.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        /// <summary>
        /// Signs the user out.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }

        /// <summary>
        /// Returns the register view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// Checks if a certain email is currently in the database.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [AcceptVerbs("Get","Post")]
        public async Task<IActionResult> IsEmailInUse(string email) 
        {
            // Find a user with the same email
            var user = await userManager.FindByEmailAsync(email);
            
            // If user is returned as null, then no other user with the same email exists
            if (user == null)
            {
                return Json(true);
            }
            else 
            {
                return Json($"Email {email} is already in user");
            }
        }

        /// <summary>
        /// Registers an ew user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {   
                    // If admin registers a new user then redirect back to the user list
                    if(signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListUsers", "Administration");
                    }
                    // Sign in the newly registered user then return to home page
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        /// <summary>
        /// Returns the login view.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// Attempts to log a user in.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else 
                    {
                        return RedirectToAction("index", "home"); 
                    }
                    
                }

                ModelState.AddModelError(string.Empty, "Email/Password may be incorrect");
            }
            return View(model);
        }

        /// <summary>
        /// Returns the access denied view for when a user enters a restriced area.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
