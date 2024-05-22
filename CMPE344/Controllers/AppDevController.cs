using CMPE344.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace CMPE344.Controllers;

[Authorize(Roles = "ROOT")]
public class AppDevController(IDatabase database) : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CreateDatabaseAsync()
    {
        await database.CreateDatabaseAsync();
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> DropDatabaseAsync()
    {
        await database.DropDatabaseAsync();
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrepareTablesProcedureAsync()
    {
        try
        {
            await database.CreatePrepareTablesProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PrepareTablesAsync()
    {
        try
        {
            await database.PrepareTablesAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreatePrepareRolesProcedure()
    {
        try
        {
            await database.CreatePrepareRolesProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateTriggersAsync()
    {
        try
        {
            await database.CreateTriggersAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> PrepareRolesAsync()
    {
        try
        {
            await database.PrepareRolesAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateCreateCustomerProcedure()
    {
        try
        {
            await database.CreateCreateCustomerProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateCreateTAProcedure()
    {
        try
        {
            await database.CreateCreateTAProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateGetUserDetailsProcedure()
    {
        try
        {
            await database.CreateGetUserDetailsProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateGetUserDetails2Procedure()
    {
        try
        {
            await database.CreateGetUserDetails2ProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateCustomerProcedure()
    {
        try
        {
            await database.CreateUpdateCustomerProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateUpdateTravelAgentProcedure()
    {
        try
        {
            await database.CreateUpdateTravelAgentProcedureAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> CreateViewsAsync()
    {
        try
        {
            await database.CreateViewsAsync();
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectUsersAsync()
    {
        try
        {
            return View("Index", await database.SelectUsersAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectCustomersAsync()
    {
        try
        {
            return View("Index", await database.SelectCustomersAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectTravelAgentsAsync()
    {
        try
        {
            return View("Index", await database.SelectTravelAgentsAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectHotelsAsync()
    {
        try
        {
            return View("Index", await database.SelectHotelsAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectFlightsAsync()
    {
        try
        {
            return View("Index", await database.SelectFlightsAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectToursAsync()
    {
        try
        {
            return View("Index", await database.SelectToursAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectUsersPreferencesAsync()
    {
        try
        {
            return View("Index", await database.SelectUsersPreferencesAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectToursWithDetailsAsync()
    {
        try
        {
            return View("Index", await database.SelectToursWithDetailsAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SelectCustomerTourBuyAsync()
    {
        try
        {
            return View("Index", await database.SelectCustomerTourBuyAsync());
        }
        catch (MySqlException ex)
        {
            ViewData["ErrorMessage"] = $"{ex.Number} - {ex.Message}";
        }
        catch (Exception ex)
        {
            ViewData["ErrorMessage"] = ex.Message;
        }
        return View("Index");
    }
}