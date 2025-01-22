using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MPBankMiniProject.Models;
using MPBankMiniProject.Models.ViewModels;

namespace MPBankMiniProject.Controllers
{
    public class AccountController : Controller
    {
        #region InjectedServices
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private RoleManager<IdentityRole> roleManager;
        private readonly IWebHostEnvironment webHostEnvironment;


        public AccountController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, RoleManager<IdentityRole> _roleManager, IWebHostEnvironment hostEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            roleManager = _roleManager;
            webHostEnvironment = hostEnvironment;
        }
        #endregion


        #region Users
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                string uniqueFile = UploadFile(model.Image);

                ApplicationUser user = new ApplicationUser()
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Balance = 200000,
                    FName = model.FName,
                    LName = model.LName,
                    Img = uniqueFile

                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) { 
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Display", "Home");
                }
                foreach(var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
                return View(model);

            
            }
            return View(model);

        }
 
        public string UploadFile(IFormFile Image) 
        {
            string uniqueFileName = null;

            if (Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Image.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    Image.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }

public IActionResult Login()
{
    return View();
}
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (ModelState.IsValid)
    {
        var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
        if (result.Succeeded)
        {
            return RedirectToAction("Display", "Home");
        }
        ModelState.AddModelError("", "Invalid user or password");
        return View(model);
    }
    return View(model);

}
        [HttpGet]
        public IActionResult Logout(int? id)
        {
            return RedirectToAction("Login", "Account");
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }



        #endregion
    }
}
