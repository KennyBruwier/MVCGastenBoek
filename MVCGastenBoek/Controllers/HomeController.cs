using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVCGastenBoek.Database;
using MVCGastenBoek.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCGastenBoek.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly GastenBoekContext _context;

        public HomeController(ILogger<HomeController> logger, UserManager<ApplicationUser>userManager, SignInManager<ApplicationUser>signInManager, GastenBoekContext context)
        {
            _logger = logger;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _context = context;
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost("/login")]
        [HttpPost("/Home/login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string username, string password)
        {
            var signInResult = await signInManager.PasswordSignInAsync(username, password, false, false);
            if (signInResult.Succeeded)
                return RedirectToAction(nameof(Index));
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string username, string password)
        {
            var result = await userManager.CreateAsync(new ApplicationUser() { UserName = username }, password);
            if (result.Succeeded)
            {
                var signInResult = await signInManager.PasswordSignInAsync(username, password, false, false);
                if (signInResult.Succeeded)
                    return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index));
        }
        //public IActionResult Authenticate()
        //{
        //    var
        //}
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

    }
}
