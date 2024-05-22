using CMPE344.Helpers;
using CMPE344.Models;
using CMPE344.ViewModels.Account;
using MySql.Data.MySqlClient;
using System.Data;

namespace CMPE344.Services;

public class Database : IDatabase
{
    private const string CONNECTION_STRING = "server=localhost;port=3306;allowuservariables=True";
    private const string CONNECTION_STRING_W_DB = $"{CONNECTION_STRING};database={DATABASE_NAME}";

    private static string ROOT_CONNECTION_STRING => $"{CONNECTION_STRING};user=root;password={ROOT_PASSWORD}";
    private static string ROOT_CONNECTION_STRING_W_DB => $"{CONNECTION_STRING_W_DB};user=root;password={ROOT_PASSWORD}";

    public static string ROOT_PASSWORD = "1234";

    public const string DATABASE_NAME = "cmpe344_project";


    public async Task<IUser?> GetUserAsync(int uid)
    {
        IUser? user = null;

        using (MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB))
        {
            await conn.OpenAsync();

            using MySqlCommand cmd = new("GetUserDetails2", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@userId", uid);
            cmd.Parameters["@userId"].Direction = ParameterDirection.Input;

            var reader = await cmd.ExecuteReaderAsync();
            var res = await reader.ReadAsync();
            if (res)
            {
                var role = reader.GetString("Role");
                if (role == "Customer")
                {
                    user = new Customer(userId: reader.GetInt32("UserId"), customerId: reader.GetInt32("CustomerId"), email: reader.GetString("Email"), firstName: reader.GetString("FirstName"), lastName: reader.GetString("LastName")) { Address = await reader.IsDBNullAsync("Address") ? null : reader.GetString("Address"), PhoneNumber = await reader.IsDBNullAsync("PhoneNumber") ? null : reader.GetString("PhoneNumber"), EmailPreference = reader.GetBoolean("EmailPreference"), PhonePreference = reader.GetBoolean("PhonePreference") };
                }
                else if (role == "Travel Agent")
                {
                    user = new TravelAgent(userId: reader.GetInt32("UserId"), agentId: reader.GetInt32("AgentId"), email: reader.GetString("Email"), firstName: reader.GetString("FirstName"), lastName: reader.GetString("LastName"), agencyName: reader.GetString("AgencyName"), commissionRate: reader.GetDouble("CommissionRate")) { Address = await reader.IsDBNullAsync("Address") ? null : reader.GetString("Address"), PhoneNumber = await reader.IsDBNullAsync("PhoneNumber") ? null : reader.GetString("PhoneNumber") };
                }
            }
        }

        return user;
    }

    public async Task<IUser?> GetUserAsync(string email, string password)
    {
        IUser? user = null;

        using (MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB))
        {
            await conn.OpenAsync();

            using MySqlCommand cmd = new("GetUserDetails", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@email", email);
            cmd.Parameters["@email"].Direction = ParameterDirection.Input;

            cmd.Parameters.AddWithValue("@password", HashHelper.GetHashString(password));
            cmd.Parameters["@password"].Direction = ParameterDirection.Input;

            var reader = await cmd.ExecuteReaderAsync();
            var res = await reader.ReadAsync();
            if (res)
            {
                var role = reader.GetString("Role");
                if (role == "Customer")
                {
                    user = new Customer(userId: reader.GetInt32("UserId"), customerId: reader.GetInt32("CustomerId"), email: reader.GetString("Email"), firstName: reader.GetString("FirstName"), lastName: reader.GetString("LastName")) { Address = await reader.IsDBNullAsync("Address") ? null : reader.GetString("Address"), PhoneNumber = await reader.IsDBNullAsync("PhoneNumber") ? null : reader.GetString("PhoneNumber"), EmailPreference = reader.GetBoolean("EmailPreference"), PhonePreference = reader.GetBoolean("PhonePreference") };
                }
                else if (role == "Travel Agent")
                {
                    user = new TravelAgent(userId: reader.GetInt32("UserId"), agentId: reader.GetInt32("AgentId"), email: reader.GetString("Email"), firstName: reader.GetString("FirstName"), lastName: reader.GetString("LastName"), agencyName: reader.GetString("AgencyName"), commissionRate: reader.GetDouble("CommissionRate")) { Address = await reader.IsDBNullAsync("Address") ? null : reader.GetString("Address"), PhoneNumber = await reader.IsDBNullAsync("PhoneNumber") ? null : reader.GetString("PhoneNumber") };
                }
            }
        }

        return user;
    }


    public async Task RegisterCustomerAsync(RegisterViewModel model)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new("CreateCustomer", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("email", model.Email);
        cmd.Parameters.AddWithValue("password", HashHelper.GetHashString(model.Password));
        cmd.Parameters.AddWithValue("first_name", model.FirstName);
        cmd.Parameters.AddWithValue("last_name", model.LastName);
        cmd.Parameters.AddWithValue("address", model.Address);
        cmd.Parameters.AddWithValue("phone_number", model.PhoneNumber);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task RegisterTAAsync(RegisterViewModel model)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new("CreateTA", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@email", model.Email);
        cmd.Parameters["@email"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@password", HashHelper.GetHashString(model.Password));
        cmd.Parameters["@password"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@first_name", model.FirstName);
        cmd.Parameters["@first_name"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@last_name", model.LastName);
        cmd.Parameters["@last_name"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@address", model.Address);
        cmd.Parameters["@address"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@phone_number", model.PhoneNumber);
        cmd.Parameters["@phone_number"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@agency_name", model.AgencyName);
        cmd.Parameters["@agency_name"].Direction = ParameterDirection.Input;

        cmd.Parameters.AddWithValue("@commission_rate", model.CommissionRate);
        cmd.Parameters["@commission_rate"].Direction = ParameterDirection.Input;

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task UpdateUserAsync(int userId, ProfileViewModel model)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"UPDATE `{DATABASE_NAME}`.`user` SET `Address` = @address, `Email` = @email, `FirstName` = @fname, `LastName` = @lname, `PhoneNumber` = @phoneNumber WHERE `UserId` = @uid", conn);
        cmd.Parameters.AddWithValue("uid", userId);
        cmd.Parameters.AddWithValue("address", model.Address);
        cmd.Parameters.AddWithValue("email", model.Email);
        cmd.Parameters.AddWithValue("fname", model.FirstName);
        cmd.Parameters.AddWithValue("lname", model.LastName);
        cmd.Parameters.AddWithValue("phoneNumber", model.PhoneNumber);

        await cmd.ExecuteNonQueryAsync();

        if (model.IsCustomer)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"UPDATE `{DATABASE_NAME}`.`preference` SET `EmailPreference` = @emailPreference, `PhonePreference` = @phonePreference WHERE `PreferenceId` = (SELECT `PreferenceId` FROM `{DATABASE_NAME}`.`customer` WHERE `UserId` = @uid)";
            cmd.Parameters.AddWithValue("uid", userId);
            cmd.Parameters.AddWithValue("emailPreference", model.EmailPreference ? (short)1 : (short)0);
            cmd.Parameters.AddWithValue("phonePreference", model.PhonePreference ? (short)1 : (short)0);

            await cmd.ExecuteNonQueryAsync();
        }
        else
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"UPDATE `{DATABASE_NAME}`.travel_agent SET AgencyName = @agencyName, CommissionRate = @commissionRate WHERE UserId = @uid";
            cmd.Parameters.AddWithValue("uid", userId);
            cmd.Parameters.AddWithValue("agencyName", model.AgencyName);
            cmd.Parameters.AddWithValue("commissionRate", model.CommissionRate);

            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task<bool> ChangePasswordAsync(int userId, ProfileViewModel model)
    {
        bool res = false;

        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"UPDATE `{DATABASE_NAME}`.`user` SET `Password` = @pwd WHERE `UserId` = @uid AND `Password` = @originalPwd", conn);
        cmd.Parameters.AddWithValue("uid", userId);
        cmd.Parameters.AddWithValue("pwd", HashHelper.GetHashString(model.NewPassword));
        cmd.Parameters.AddWithValue("originalPwd", HashHelper.GetHashString(model.OriginalPassword));

        int rowsAffected = await cmd.ExecuteNonQueryAsync();

        if (rowsAffected == 1)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"ALTER USER 'User_{userId}'@'localhost' IDENTIFIED WITH mysql_native_password BY '{HashHelper.GetHashString(model.NewPassword)}'";
            await cmd.ExecuteNonQueryAsync();
            res = true;
        }

        return res;
    }


    public async Task<List<Tour>> GetToursAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"SELECT `TourId`, `Title`, `Description`, `StartDate`, `EndDate`, `Capacity`, `Price`, `HotelId`, `FlightId`, `CreatedBy`, `Applied` FROM `{DATABASE_NAME}`.`tourwithdetails`", conn);

        var reader = await cmd.ExecuteReaderAsync();

        List<Tour> tours = [];

        while (await reader.ReadAsync())
        {
            tours.Add(new Tour
            {
                Capacity = reader.GetInt32("Capacity"),
                Description = await reader.IsDBNullAsync("Description") ? null : reader.GetString("Description"),
                EndDate = reader.GetDateTime("EndDate"),
                Price = reader.GetDouble("Price"),
                StartDate = reader.GetDateTime("StartDate"),
                Title = reader.GetString("Title"),
                TourId = reader.GetInt32("TourId"),
                Applied = reader.GetInt32("Applied"),
                HotelId = reader.GetInt32("HotelId"),
                FlightId = reader.GetInt32("FlightId"),
                CreatedBy = reader.GetInt32("CreatedBy"),
            });
        }

        return tours;
    }


    public async Task<int?> CreateHotel(string name, string location)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"INSERT INTO `{DATABASE_NAME}`.`hotel` (`HotelName`, `Location`) VALUES (@hotelName, @location)", conn);
        cmd.Parameters.AddWithValue("hotelName", name);
        cmd.Parameters.AddWithValue("location", location);

        await cmd.ExecuteNonQueryAsync();

        return (int?)cmd.LastInsertedId;
    }

    public async Task<Hotel?> GetHotelAsync(int hotelId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"SELECT `HotelId`, `HotelName`, `Location` FROM `{DATABASE_NAME}`.`hotel` WHERE `HotelId` = @hId", conn);
        cmd.Parameters.AddWithValue("hId", hotelId);

        var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Hotel
            {
                HotelId = reader.GetInt32("HotelId"),
                HotelName = reader.GetString("HotelName"),
                Location = reader.GetString("Location")
            };
        }

        return null;
    }

    public async Task UpdateHotelAsync(int hotelId, string name, string location)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"UPDATE `{DATABASE_NAME}`.`hotel` SET `HotelName` = @hotelName, `Location` = @location WHERE `HotelId` = @hId", conn);
        cmd.Parameters.AddWithValue("hId", hotelId);
        cmd.Parameters.AddWithValue("hotelName", name);
        cmd.Parameters.AddWithValue("location", location);

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task<int?> CreateFlightAsync(string origin, string destination, string airline, DateTime departureTime, DateTime arrivalTime)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"INSERT INTO `{DATABASE_NAME}`.`flight` (`Origin`, `Destination`, `Airline`, `DepartureTime`, `ArrivalTime`) VALUES (@origin, @destination, @airline, @departureTime, @arrivalTime)", conn);
        cmd.Parameters.AddWithValue("origin", origin);
        cmd.Parameters.AddWithValue("destination", destination);
        cmd.Parameters.AddWithValue("airline", airline);
        cmd.Parameters.AddWithValue("departureTime", departureTime);
        cmd.Parameters.AddWithValue("arrivalTime", arrivalTime);

        await cmd.ExecuteNonQueryAsync();

        return (int?)cmd.LastInsertedId;
    }

    public async Task<Flight?> GetFlightAsync(int flightId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"SELECT `FlightId`, `Origin`, `Destination`, `Airline`, `DepartureTime`, `ArrivalTime` FROM `{DATABASE_NAME}`.`flight` WHERE `FlightId` = @fId", conn);
        cmd.Parameters.AddWithValue("fId", flightId);

        var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Flight
            {
                FlightId = reader.GetInt32("FlightId"),
                Origin = reader.GetString("Origin"),
                Destination = reader.GetString("Destination"),
                Airline = reader.GetString("Airline"),
                DepartureTime = reader.GetDateTime("DepartureTime"),
                ArrivalTime = reader.GetDateTime("ArrivalTime"),
            };
        }

        return null;
    }

    public async Task UpdateFlightAsync(int flightId, string origin, string destination, string airline, DateTime departureTime, DateTime arrivalTime)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"UPDATE `{DATABASE_NAME}`.`flight` SET `Origin` = @origin, `Destination` = @destination, `Airline` = @airline, `DepartureTime` = @departureTime, `ArrivalTime` = @arrivalTime WHERE `FlightId` = @fId", conn);
        cmd.Parameters.AddWithValue("fId", flightId);
        cmd.Parameters.AddWithValue("origin", origin);
        cmd.Parameters.AddWithValue("destination", destination);
        cmd.Parameters.AddWithValue("airline", airline);
        cmd.Parameters.AddWithValue("departureTime", departureTime);
        cmd.Parameters.AddWithValue("arrivalTime", arrivalTime);

        await cmd.ExecuteNonQueryAsync();
    }


    public async Task<int?> CreateTourAsync(string title, string? description, DateTime startDate, DateTime endDate, int capacity, double price, int hotelId, int flightId, int createdByUser)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
        }

        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"SELECT `AgentId` FROM `{DATABASE_NAME}`.`travel_agent` INNER JOIN `{DATABASE_NAME}`.`user` USING (`UserId`) WHERE `UserId` = @uid", conn);
        cmd.Parameters.AddWithValue("uid", createdByUser);
        int? agentId = (int?)await cmd.ExecuteScalarAsync();

        if (agentId != null)
        {
            cmd.Parameters.Clear();
            cmd.CommandText = $"INSERT INTO `{DATABASE_NAME}`.`tour` (`Title`, `Description`, `StartDate`, `EndDate`, `Capacity`, `Price`, `HotelId`, `FlightId`, `CreatedBy`) VALUES (@title, @description, @startDate, @endDate, @capacity, @price, @hotelId, @flightId, @agentId)";
            cmd.Parameters.AddWithValue("title", title);
            cmd.Parameters.AddWithValue("description", description);
            cmd.Parameters.AddWithValue("startDate", startDate);
            cmd.Parameters.AddWithValue("endDate", endDate);
            cmd.Parameters.AddWithValue("capacity", capacity);
            cmd.Parameters.AddWithValue("price", price);
            cmd.Parameters.AddWithValue("hotelId", hotelId);
            cmd.Parameters.AddWithValue("flightId", flightId);
            cmd.Parameters.AddWithValue("agentId", agentId);

            await cmd.ExecuteNonQueryAsync();

            return (int?)cmd.LastInsertedId;
        }

        return null;
    }

    public async Task<Tour?> GetTourAsync(int tourId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"SELECT `TourId`, `Title`, `Description`, `StartDate`, `EndDate`, `Capacity`, `Price`, `HotelId`, `FlightId`, `CreatedBy`, `Applied` FROM `{DATABASE_NAME}`.`tourwithdetails` WHERE `TourId` = @tId", conn);
        cmd.Parameters.AddWithValue("tId", tourId);

        var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            return new Tour
            {
                TourId = reader.GetInt32("TourId"),
                Title = reader.GetString("Title"),
                Description = await reader.IsDBNullAsync("Description") ? null : reader.GetString("Description"),
                StartDate = reader.GetDateTime("StartDate"),
                EndDate = reader.GetDateTime("EndDate"),
                Capacity = reader.GetInt32("Capacity"),
                Price = reader.GetDouble("Price"),
                HotelId = reader.GetInt32("HotelId"),
                FlightId = reader.GetInt32("FlightId"),
                CreatedBy = reader.GetInt32("CreatedBy"),
                Applied = reader.GetInt32("Applied")
            };
        }

        return null;
    }

    public async Task DeleteTourAsync(int tourId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"DELETE FROM `{DATABASE_NAME}`.`tour` WHERE `TourId` = @tId", conn);
        cmd.Parameters.AddWithValue("tId", tourId);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task BuyTourAsync(int userId, int tourId)
    {
        var tour = await GetTourAsync(tourId);

        if (tour != null && tour.RemainingQuota > 0)
        {
            using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
            await conn.OpenAsync();

            using MySqlCommand cmd = new($"SELECT `CustomerId` FROM `{DATABASE_NAME}`.`customer` WHERE `UserId` = @uid", conn);
            cmd.Parameters.AddWithValue("uid", userId);
            int? customerId = (int?)await cmd.ExecuteScalarAsync();

            if (customerId != null)
            {
                cmd.Parameters.Clear();
                cmd.CommandText = $"INSERT INTO `{DATABASE_NAME}`.`customer_tour_buy` (`CustomerId`, `TourId`, `ApplyDate`) VALUES (@cId, @tId, @applyDate)";
                cmd.Parameters.AddWithValue("cId", customerId);
                cmd.Parameters.AddWithValue("tId", tourId);
                cmd.Parameters.AddWithValue("applyDate", DateTime.Now);

                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<bool> CheckCustomerTourBuy(int userId, int tourId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();


        using MySqlCommand cmd = new($"SELECT `CustomerId` FROM `{DATABASE_NAME}`.`customer` WHERE `UserId` = @uid", conn);
        cmd.Parameters.AddWithValue("uid", userId);
        int? customerId = (int?)await cmd.ExecuteScalarAsync();

        cmd.Parameters.Clear();
        cmd.CommandText = $"SELECT 1 FROM `{DATABASE_NAME}`.`customer_tour_buy` WHERE `CustomerId` = @cId AND `TourId` = @tId";
        cmd.Parameters.AddWithValue("cId", customerId);
        cmd.Parameters.AddWithValue("tId", tourId);

        var val = (long?)await cmd.ExecuteScalarAsync();

        return val != null && val == 1;
    }

    public async Task ReturnTourAsync(int userId, int tourId)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();


        using MySqlCommand cmd = new($"SELECT `CustomerId` FROM `{DATABASE_NAME}`.`customer` WHERE `UserId` = @uid", conn);
        cmd.Parameters.AddWithValue("uid", userId);
        int? customerId = (int?)await cmd.ExecuteScalarAsync();

        cmd.Parameters.Clear();
        cmd.CommandText = $"DELETE FROM `{DATABASE_NAME}`.`customer_tour_buy` WHERE `CustomerId` = @cId AND `TourId` = @tId";
        cmd.Parameters.AddWithValue("cId", customerId);
        cmd.Parameters.AddWithValue("tId", tourId);

        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateTourAsync(int tourId, string title, string? description, DateTime startDate, DateTime endDate, int capacity, double price)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"UPDATE `{DATABASE_NAME}`.`tour` SET `Title` = @title, `Description` = @description, `StartDate` = @startDate, `EndDate` = @endDate, `Capacity` = @capacity, `Price` = @price WHERE `TourId` = @tId", conn);

        cmd.Parameters.AddWithValue("tId", tourId);
        cmd.Parameters.AddWithValue("title", title);
        cmd.Parameters.AddWithValue("description", description);
        cmd.Parameters.AddWithValue("startDate", startDate);
        cmd.Parameters.AddWithValue("endDate", endDate);
        cmd.Parameters.AddWithValue("capacity", capacity);
        cmd.Parameters.AddWithValue("price", price);

        var res = await cmd.ExecuteNonQueryAsync();
    }

    #region Database Management

    public async Task CreateDatabaseAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"CREATE DATABASE IF NOT EXISTS`{DATABASE_NAME}`", conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DropDatabaseAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING);
        await conn.OpenAsync();

        using MySqlCommand cmd = new($"DROP DATABASE IF EXISTS `{DATABASE_NAME}`", conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task PrepareRolesAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new("PrepareRoles", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task PrepareTablesAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new("PrepareTables", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        await cmd.ExecuteNonQueryAsync();
    }

    #region Create Procedures

    public async Task CreatePrepareTablesProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.PrepareTablesProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreatePrepareRolesProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.PrepareRolesProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateCreateCustomerProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.CreateCustomerProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateCreateTAProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.CreateTAProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateGetUserDetailsProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.GetUserDetailsProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateGetUserDetails2ProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.GetUserDetails2Procedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateUpdateCustomerProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.UpdateCustomerProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateUpdateTravelAgentProcedureAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.UpdateTravelAgentProcedure, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    #endregion

    public async Task CreateTriggersAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.TourBeforeDeleteTrigger, conn);
        await cmd.ExecuteNonQueryAsync();

        cmd.CommandText = ConstantProcedures.TourAfterDeleteTrigger;
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task CreateViewsAsync()
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlCommand cmd = new(ConstantProcedures.TourWithDetailsView, conn);
        await cmd.ExecuteNonQueryAsync();
    }

    #region Select Tables

    public async Task<DataTable> SelectUsersAsync() => await SelectTableAsync("user");

    public async Task<DataTable> SelectCustomersAsync() => await SelectTableAsync("Customer");

    public async Task<DataTable> SelectTravelAgentsAsync() => await SelectTableAsync("travel_agent");

    public async Task<DataTable> SelectHotelsAsync() => await SelectTableAsync("hotel");

    public async Task<DataTable> SelectFlightsAsync() => await SelectTableAsync("flight");

    public async Task<DataTable> SelectToursAsync() => await SelectTableAsync("tour");

    public async Task<DataTable> SelectUsersPreferencesAsync() => await SelectTableAsync("preference");

    public async Task<DataTable> SelectToursWithDetailsAsync() => await SelectTableAsync("tourwithdetails");

    public async Task<DataTable> SelectCustomerTourBuyAsync() => await SelectTableAsync("customer_tour_buy");

    public async Task<DataTable> SelectTableAsync(string tableName)
    {
        using MySqlConnection conn = new(ROOT_CONNECTION_STRING_W_DB);
        await conn.OpenAsync();

        using MySqlDataAdapter msda = new($"SELECT * FROM `{DATABASE_NAME}`.`{tableName}`", conn);

        DataTable dt = new();

        await msda.FillAsync(dt);

        return dt;
    }

    #endregion

    #endregion
}