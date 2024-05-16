using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Your_Wallet.Areas.Identity.Data;
using Your_Wallet.Helpers;
using Your_Wallet.Models.ViewModels;

namespace Your_Wallet.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: true);
                    if (!user.NameSet)
                    {
                        return RedirectToAction("AdditionalInfo");
                    }
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (!user.NameSet)
                    {
                        return RedirectToAction("AdditionalInfo");
                    }
                    // Redirect to some other page if needed
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult AdditionalInfo()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AdditionalInfo(AdditionalInfoModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if the model state is valid and save the additional information
                var user = await _userManager.GetUserAsync(User); // Get the current user
                if (user != null)
                {
                    // Save additional information to the user entity
                    user.Name = model.Name;
                    user.StartingBalance = model.StartingBalance;

                    // Upload profile picture if provided
                    if (model.Image != null)
                    {
                        // Upload profile picture and update ProfilePicture property
                        user.ProfilePicture = await DocumentSettings.UploadFile(model.Image, "profilePictures");
                    }

                    // Set NameSet flag to true indicating that the user's name is set
                    user.NameSet = true;

                    // Update user entity
                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        // Redirect to some other page if needed
                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    return NotFound(); // User not found
                }
            }
            return View(model);
        }

        // Logout

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
