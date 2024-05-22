using CMPE344.Models;
using CMPE344.ViewModels.Account;
using System.Data;

namespace CMPE344.Services;

public interface IDatabase
{

    Task<IUser?> GetUserAsync(string email, string password);
    Task<IUser?> GetUserAsync(int userId);

    Task RegisterCustomerAsync(RegisterViewModel model);
    Task RegisterTAAsync(RegisterViewModel model);

    Task UpdateUserAsync(int userId, ProfileViewModel model);

    Task<bool> ChangePasswordAsync(int userId, ProfileViewModel model);

    Task<List<Tour>> GetToursAsync();

    Task<int?> CreateHotel(string name, string location);
    Task<Hotel?> GetHotelAsync(int hotelId);
    Task UpdateHotelAsync(int hotelId, string name, string location);

    Task<int?> CreateFlightAsync(string origin, string destination, string airline, DateTime departureTime, DateTime arrivalTime);
    Task<Flight?> GetFlightAsync(int flightId);
    Task UpdateFlightAsync(int flightId, string origin, string destination, string airline, DateTime departureTime, DateTime arrivalTime);

    Task<int?> CreateTourAsync(string title, string? description, DateTime startDate, DateTime endDate, int capacity, double price, int hotelId, int flightId, int createdByUser);
    Task<Tour?> GetTourAsync(int tourId);
    Task DeleteTourAsync(int tourId);
    Task BuyTourAsync(int userId, int tourId);
    Task<bool> CheckCustomerTourBuy(int userId, int tourId);
    Task ReturnTourAsync(int userId, int tourId);
    Task UpdateTourAsync(int tourId, string title, string? description, DateTime startDate, DateTime endDate, int capacity, double price);
    
    Task CreateDatabaseAsync();
    Task DropDatabaseAsync();

    Task PrepareRolesAsync();

    Task PrepareTablesAsync();

    Task CreateViewsAsync();

    Task<DataTable> SelectUsersAsync();
    Task<DataTable> SelectCustomersAsync();
    Task<DataTable> SelectTravelAgentsAsync();
    Task<DataTable> SelectHotelsAsync();
    Task<DataTable> SelectFlightsAsync();
    Task<DataTable> SelectToursAsync();
    Task<DataTable> SelectUsersPreferencesAsync();
    Task<DataTable> SelectToursWithDetailsAsync();
    Task<DataTable> SelectCustomerTourBuyAsync();
    Task<DataTable> SelectTableAsync(string tableName);

    Task CreatePrepareTablesProcedureAsync();
    Task CreatePrepareRolesProcedureAsync();
    Task CreateCreateCustomerProcedureAsync();
    Task CreateCreateTAProcedureAsync();
    Task CreateGetUserDetailsProcedureAsync();
    Task CreateGetUserDetails2ProcedureAsync();
    Task CreateUpdateCustomerProcedureAsync();
    Task CreateUpdateTravelAgentProcedureAsync();
    Task CreateTriggersAsync();
}