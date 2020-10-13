using System.Diagnostics;
using System.Threading.Tasks;
using Forum_Project.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Forum_Project.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<ApplicationUser> userManager;

        public HomeController(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, ILogger<HomeController> logger)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// Allows user to switch between user and admin. Should be deleted before completion.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Initialize()
        {
            var user = await userManager.GetUserAsync(User);

            // If user is not an admin, add to admin role
            if (!User.IsInRole("Admin"))
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            // If user is an admin, remove them from the admin role and add them to user incase they are not already one
            else
            {
                await userManager.RemoveFromRoleAsync(user, "Admin");
                await userManager.AddToRoleAsync(user, "User");
            }

            return View("Index");
        }
    }
}
