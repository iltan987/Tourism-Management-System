using CMPE344.Models;
using CMPE344.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace CMPE344.Controllers;
public class HomeController(IDatabase database) : Controller
{
    // GET: /
    [HttpGet]
    public async Task<IActionResult> IndexAsync()
    {
        // Check if the user is already authenticated
        if (User.Identity?.IsAuthenticated == true)
        {
            // Check if the authenticated user has both UserId and Email claims
            if (User.HasClaim(f => f.Type == "UserId") && User.HasClaim(f => f.Type == ClaimTypes.Email))
            {
                // Retrieve the user from the database using the UserId claim
                var user = await database.GetUserAsync(int.Parse(User.Claims.First(f => f.Type == "UserId").Value));

                // If the user is not found in the database, sign out and redirect to the Index action
                if (user == null)
                {
                    await HttpContext.SignOutAsync();
                    return RedirectToAction("Index");
                }
            }
        }

        // Return the default view for the Index action
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}