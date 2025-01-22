using System.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPBankMiniProject.Models;

namespace MPBankMiniProject.Controllers
{
    public class HomeController : Controller
    {
        #region InjectedServices
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;

        public HomeController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager)
        {
            userManager = _userManager;
            signInManager = _signInManager;
        }

        #endregion

        public async Task<IActionResult> Display()
        {
            var r =  await userManager.GetUserAsync(User);
            
            return View(r);
        }

        [HttpGet]
        public async Task<IActionResult> Transaction()
        {
            var r = await userManager.GetUserAsync(User);
            return View();
        }

    }
}
