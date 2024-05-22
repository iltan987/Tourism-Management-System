using CMPE344.Helpers;
using CMPE344.Models;
using CMPE344.Services;
using CMPE344.ViewModels.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Security.Claims;

namespace CMPE344.Controllers;
public class AccountController(IDatabase database) : Controller
{
    // GET: /Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: /Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        if (HashHelper.GetHashString(model.Email) == "07B594BFFB1E8306406747D021222A99A950A89C57539C5FD72D4F3AD40B0CE2") // root@root
        {
            if (HashHelper.GetHashString(model.Password) == "15E2B0D3C33891EBB0F1EF609EC419420C20E320CE94C65FBC8C3312448EB225") // 123456789
            {
                await SignInAsync(new Claim(ClaimTypes.Name, "root"), new Claim(ClaimTypes.Role, "ROOT"));
                return RedirectToAction("Index", "AppDev");
            }
        }
        if (ModelState.IsValid)
        {
            var userDetails = await database.GetUserAsync(model.Email, model.Password);
            if (userDetails != null)
            {
                await SignInAsync(userDetails.UserId, model.Email, $"{userDetails.FirstName} {userDetails.LastName}", userDetails is Customer ? "Customer" : "Travel Agent");
                return RedirectToAction("Index", "Home");
            }

            AddErrors("Email or Password is not correct");
        }
        return View(model);
    }

    private async Task SignInAsync(params Claim[] claims)
    {
        ClaimsIdentity identity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new(identity);
        await HttpContext.SignInAsync(principal);
    }

    private async Task SignInAsync(int userId, string email, string name, string role)
    {
        await SignInAsync(new Claim("UserId", userId.ToString()),
            new Claim(ClaimTypes.Email, email),
            new Claim(ClaimTypes.Name, name),
            new Claim(ClaimTypes.Role, role));
    }

    // GET: /Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register()
    {
        if (User.Identity?.IsAuthenticated == true)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> RegisterAsync(RegisterViewModel model)
    {
        if (model.UserType != "Travel Agent")
        {
            ModelState.Remove(nameof(RegisterViewModel.AgencyName));
            ModelState.Remove(nameof(RegisterViewModel.CommissionRate));
        }

        if (ModelState.IsValid)
        {
            try
            {
                if (model.UserType == "Customer")
                {
                    await database.RegisterCustomerAsync(model);
                }
                else
                {
                    await database.RegisterTAAsync(model);
                }
                return RedirectToAction("Login", "Account");
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                {
                    AddErrors("This email is already registered.");
                }
                else
                {
                    AddErrors(ex.Number + " - " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                AddErrors(ex.Message);
            }
        }

        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> LogoutAsync()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Login", "Account");
    }

    // POST: /Account/EditProfile
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditProfileAsync(ProfileViewModel model)
    {
        ModelState.Remove(nameof(ProfileViewModel.OriginalPassword));
        ModelState.Remove(nameof(ProfileViewModel.NewPassword));
        ModelState.Remove(nameof(ProfileViewModel.ConfirmNewPassword));

        model.IsCustomer = User.IsInRole("Customer");

        if (model.IsCustomer)
        {
            ModelState.Remove(nameof(ProfileViewModel.AgencyName));
            ModelState.Remove(nameof(ProfileViewModel.CommissionRate));
        }
        else
        {
            ModelState.Remove(nameof(ProfileViewModel.EmailPreference));
            ModelState.Remove(nameof(ProfileViewModel.PhonePreference));
        }

        if (ModelState.IsValid)
        {
            int userId = int.Parse(User.Claims.First(f => f.Type == "UserId").Value);
            await database.UpdateUserAsync(userId, model);

            await SignInAsync(new Claim("UserId", userId.ToString()), new Claim(ClaimTypes.Email, model.Email), new Claim(ClaimTypes.Name, $"{model.FirstName} {model.LastName}"), new Claim(ClaimTypes.Role, model.IsCustomer ? "Customer" : "Travel Agent"));
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    // POST: /Account/ChangePassword
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePasswordAsync(ProfileViewModel model)
    {
        ModelState.Remove(nameof(ProfileViewModel.Address));
        ModelState.Remove(nameof(ProfileViewModel.AgencyName));
        ModelState.Remove(nameof(ProfileViewModel.CommissionRate));
        ModelState.Remove(nameof(ProfileViewModel.Email));
        ModelState.Remove(nameof(ProfileViewModel.FirstName));
        ModelState.Remove(nameof(ProfileViewModel.LastName));
        ModelState.Remove(nameof(ProfileViewModel.PhoneNumber));

        if (ModelState.IsValid)
        {
            int userId = int.Parse(User.Claims.First(f => f.Type == "UserId").Value);
            bool res = await database.ChangePasswordAsync(userId, model);
            if (!res)
            {
                AddErrors("Password is incorrect");
                return View("Profile", model);
            }
            return RedirectToAction("Index", "Home");
        }
        return View(model);
    }

    public IActionResult AccessDenied()
    {
        return PartialView();
    }

    #region Helpers
    private void AddErrors(params string[] result)
    {
        foreach (var error in result)
        {
            ModelState.AddModelError("", error);
        }
    }
    #endregion

    // GET: /Account/Profile
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> ProfileAsync()
    {
        var userData = await database.GetUserAsync(int.Parse(User.Claims.First(f => f.Type == "UserId").Value));
        ProfileViewModel profileViewModel = new();
        if (userData is Customer customer)
        {
            profileViewModel.Address = customer.Address;
            profileViewModel.Email = customer.Email;
            profileViewModel.FirstName = customer.FirstName;
            profileViewModel.LastName = customer.LastName;
            profileViewModel.PhoneNumber = customer.PhoneNumber;
            profileViewModel.EmailPreference = customer.EmailPreference;
            profileViewModel.PhonePreference = customer.PhonePreference;
            profileViewModel.IsCustomer = true;
        }
        else if (userData is TravelAgent travelAgent)
        {
            profileViewModel.Address = travelAgent.Address;
            profileViewModel.AgencyName = travelAgent.AgencyName;
            profileViewModel.CommissionRate = travelAgent.CommissionRate;
            profileViewModel.Email = travelAgent.Email;
            profileViewModel.FirstName = travelAgent.FirstName;
            profileViewModel.LastName = travelAgent.LastName;
            profileViewModel.PhoneNumber = travelAgent.PhoneNumber;
            profileViewModel.IsCustomer = false;
        }
        return View(profileViewModel);
    }
}