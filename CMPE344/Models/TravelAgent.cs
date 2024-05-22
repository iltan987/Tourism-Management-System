using System.ComponentModel.DataAnnotations;

namespace CMPE344.Models;

public class TravelAgent(int userId, int agentId, string email, string firstName, string lastName, string agencyName, double commissionRate) : IUser
{
    public int UserId { get; set; } = userId;

    [Key]
    public int AgentId { get; set; } = agentId;

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

    [Required]
    [StringLength(255)]
    public string AgencyName { get; set; } = agencyName ?? throw new ArgumentNullException(nameof(agencyName));

    [Required]
    [StringLength(255)]
    public double CommissionRate { get; set; } = commissionRate;
}
