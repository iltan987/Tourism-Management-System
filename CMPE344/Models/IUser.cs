namespace CMPE344.Models;

public interface IUser
{
    int UserId { get; set; }
    string Email { get; set; }
    string FirstName { get; set; }
    string LastName { get; set; }
    string? Address { get; set; }
    string? PhoneNumber { get; set; }
}