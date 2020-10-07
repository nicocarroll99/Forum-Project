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
        /// Seeds data for our group users and sets the roles to admin. Should be deleted before completion.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Initialize()
        {
            IdentityRole identityRole = new IdentityRole
            {
                Name = "Admin"
            };

            await roleManager.CreateAsync(identityRole);

            ApplicationUser[] users = new ApplicationUser[5];

            users[0] = new ApplicationUser {UserName = "joshcarter@gmail.com", Email = "joshcarter@gmail.com"};
            users[1] = new ApplicationUser { UserName = "nicholascarroll@gmail.com", Email = "nicholascarroll@gmail.com" };
            users[2] = new ApplicationUser { UserName = "minhhua@gmail.com", Email = "minhhua@gmail.com" };
            users[3] = new ApplicationUser { UserName = "dylanlawson@gmail.com", Email = "dylanlawson@gmail.com" };
            users[4] = new ApplicationUser { UserName = "jonathanpollock@gmail.com", Email = "jonathanpollock@gmail.com" };

            foreach (var user in users)
            {
                var result = await userManager.CreateAsync(user, "temppassword440");
            }

            foreach (var user in users)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }

            return View("Index");
        }
    }
}
