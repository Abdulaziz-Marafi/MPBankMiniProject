using System.Diagnostics;
using System.Reflection;
using System.Transactions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using MPBankMiniProject.Data;
using MPBankMiniProject.Models;
using MPBankMiniProject.Models.ViewModels;

namespace MPBankMiniProject.Controllers
{
    public class HomeController : Controller
    {
        #region InjectedServices
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> signInManager;
        private ApplicatioDbContext db;
        private readonly IWebHostEnvironment webHostEnvironment;

        public HomeController(UserManager<ApplicationUser> _userManager, SignInManager<ApplicationUser> _signInManager, ApplicatioDbContext _applicatioDbContext, IWebHostEnvironment hostEnvironment)
        {
            userManager = _userManager;
            signInManager = _signInManager;
            db = _applicatioDbContext;
            webHostEnvironment = hostEnvironment;

        }

        #endregion

        public async Task<IActionResult> Display()
        {
            // ADD view Model
            var r = await userManager.GetUserAsync(User);
            if (r == null)
            {
                return NotFound();
            }
            DisplayViewModel model = new DisplayViewModel()
            {
                Balance = r.Balance,
                Email = r.Email,
                FName = r.FName,
                LName = r.LName,
                Img = r.Img,
                Id = r.Id
            };


            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Transaction()
        {
            // Add View Model
            var r = await userManager.GetUserAsync(User);
            if (r == null) { return NotFound(); }
            ViewBag.Balance = r.Balance;
            return View();
        }
        
        [HttpPost]
        public async Task<IActionResult> Transaction(UserTransactionViewModel model, string submit)
        {
            var r = await userManager.GetUserAsync(User);
            if (r == null) { return NotFound(); }
            if (ModelState.IsValid)
            {

                if (!(submit == "Withdraw" && (r.Balance - model.Amount) < 0))
                {

                    if (submit == "Withdraw")
                    {
                        Models.Transaction tran = new Models.Transaction()
                        {
                            Amount = model.Amount,
                            Type = Models.Transaction.TransactionType.Withdraw,
                            TransactionDate = model.TransactionDate,
                            ApplicationUserId = r.Id
                        };
                        db.Transactions.Add(tran);
                        db.SaveChanges();
                        r.Balance -= model.Amount;
                        var result = await userManager.UpdateAsync(r);
                        if (result.Succeeded)
                        {
                            ViewBag.Balance = r.Balance;

                            return View();
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            ViewBag.Balance = r.Balance;
                            return View(model);
                        }

                    }
                    else
                    {
                        Models.Transaction tran = new Models.Transaction()
                        {
                            Amount = model.Amount,
                            Type = Models.Transaction.TransactionType.Deposit,
                            TransactionDate = model.TransactionDate,
                            ApplicationUserId = r.Id
                        };
                        db.Transactions.Add(tran);
                        db.SaveChanges();
                        r.Balance += model.Amount;
                        var result = await userManager.UpdateAsync(r);
                        if (result.Succeeded)
                        {
                            ViewBag.Balance = r.Balance;

                            return View();
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError(error.Code, error.Description);
                            }
                            ViewBag.Balance = r.Balance;
                            return View(model);
                        }
                    }

                }
                else
                {
                    ModelState.AddModelError("Insufficient", "You can not withdraw more than the available balance.");
                    ViewBag.Balance = r.Balance;
                    return View(model);
                }
            }

            ViewBag.Balance = r.Balance;
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile()
        {
            var r = await userManager.GetUserAsync(User);
            if (r == null) { return NotFound(); }

            EditViewModel model = new EditViewModel()
            {
                Balance = r.Balance,
                Email = r.Email,
                FName = r.FName,
                LName = r.LName,
                Img = r.Img,
                Id = r.Id
            };
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.GetUserAsync(User);
                if (user == null)
                {
                    return NotFound();
                }

                user.FName = model.FName;
                user.LName = model.LName;
                user.Email = model.Email;

                if (model.NewImg != null)
                {
                    string uniqueFile = UploadFile(model.NewImg);
                    user.Img = uniqueFile;
                }
                var result = await userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Display");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> DisplayAllUsers()
        {
            var curr = await userManager.GetUserAsync(User);
            var users = userManager.Users.Where(user => user.Id != curr.Id)
            .Select(user => new DisplayViewModel
            {
                Balance = user.Balance,
                Email = user.Email,
                FName = user.FName,
                LName = user.LName,
                Img = user.Img,
                Id = user.Id
            }).ToList();

            return View(users);
        }
        public async Task<IActionResult> TransactionsList(string sortOrder, string typeFilter)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.TypeFilter = typeFilter;
            var curr = await userManager.GetUserAsync(User);
            if (curr == null)
            {
                return NotFound();
            }

            var transactions = db.Transactions.Where(tran => tran.ApplicationUserId == curr.Id).AsQueryable();

            if (!string.IsNullOrEmpty(typeFilter) && Enum.TryParse(typeFilter, out Models.Transaction.TransactionType parsedType))
            {
                transactions = transactions.Where(t => t.Type == parsedType);
            }

            switch (sortOrder)
            {
                case "amount_desc":
                    transactions = transactions.OrderByDescending(t => t.Amount);
                    break;
                case "Amount":
                    transactions = transactions.OrderBy(t => t.Amount);
                    break;
                case "date_desc":
                    transactions = transactions.OrderByDescending(t => t.TransactionDate);
                    break;
                case "Date":
                    transactions = transactions.OrderBy(t => t.TransactionDate);
                    break;
                default:
                    transactions = transactions.OrderBy(t => t.TransactionDate);
                    break;
            }

            var model = transactions.Select(t => new UserTransactionViewModel
            {
                TransactionDate = t.TransactionDate,
                Amount = t.Amount,
                Type = (UserTransactionViewModel.TransactionType)t.Type
            }).ToList();

            return View(model);
        }
        

        public async Task<IActionResult> Transfer(string? id)
        {
            var curr = await userManager.GetUserAsync(User);

            TransferViewModel transfer = new TransferViewModel()
            {
                Id = curr.Id,
                TransferieId = id,
                Amount = 0
            };
            ViewBag.Balance = curr.Balance;
            return View(transfer);
        }
        [HttpPost]
        public async Task<IActionResult> Transfer(TransferViewModel model)
        {
            var curr = await userManager.GetUserAsync(User);
            var transferie = await userManager.FindByIdAsync(model.TransferieId);
            if (transferie == null)
            {
                ModelState.AddModelError("UserNotFound", "User Not Found");
                ViewBag.Balance = curr.Balance;
                return View(model);
            }
            if (ModelState.IsValid)
            {
                if (curr.Balance - model.Amount < 0)
                {
                    ModelState.AddModelError("Insufficient", "You can not transfer more than the available balance.");
                    ViewBag.Balance = curr.Balance;
                    return View(model);
                }
                else
                {
                    Models.Transaction tran = new Models.Transaction()
                    {
                        Amount = model.Amount,
                        Type = Models.Transaction.TransactionType.Transfer,
                        TransactionDate = DateTime.Now,
                        ApplicationUserId = curr.Id,
                     
                    };
                    db.Transactions.Add(tran);
                    db.SaveChanges();
                    curr.Balance -= model.Amount;
                    transferie.Balance += model.Amount;
                    var result = await userManager.UpdateAsync(curr);
                    var result2 = await userManager.UpdateAsync(transferie);
                    if (result.Succeeded && result2.Succeeded)
                    {
                        ViewBag.Balance = curr.Balance;
                        return RedirectToAction("DisplayAllUsers");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }
                        ViewBag.Balance = curr.Balance;
                        return View(model);
                    }
                }
            }
            ViewBag.Balance = curr.Balance;
            return View(model);
        }
        public string UploadFile(IFormFile? Image)
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
    }
}
