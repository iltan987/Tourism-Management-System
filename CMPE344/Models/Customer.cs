using System.ComponentModel.DataAnnotations;

namespace CMPE344.Models;

public class Customer(int userId, int customerId, string email, string firstName, string lastName) : IUser
{
    public int UserId { get; set; } = userId;

    [Key]
    public int CustomerId { get; set; } = customerId;

    [Required]
    [StringLength(255)]
    public string Email { get; set; } = email ?? throw new ArgumentNullException(nameof(email));

    [Required]
    [StringLength(255)]
    public string FirstName { get; set; } = firstName ?? throw new ArgumentNullException(nameof(firstName));

    [Required]
    [StringLength(255)]
    public string LastName { get; set; } = lastName ?? throw new ArgumentNullException(nameof(lastName));

    [StringLength(255)]
    public string? Address { get; set; }

    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    public bool EmailPreference { get; set; } = true;

    public bool PhonePreference { get; set; } = true;
}