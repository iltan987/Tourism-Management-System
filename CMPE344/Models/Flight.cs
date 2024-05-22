using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMPE344.Models;

public class Flight
{
    [Key]
    public int FlightId { get; set; }

    [Required]
    [DisplayName("Origin")]
    [StringLength(255)]
    public string Origin { get; set; } = string.Empty;

    [Required]
    [DisplayName("Destination")]
    [StringLength(255)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [DisplayName("Airline")]
    [StringLength(255)]
    public string Airline { get; set; } = string.Empty;

    [Required]
    [DisplayName("Departure")]
    public DateTime DepartureTime { get; set; }

    [Required]
    [DisplayName("Arrival")]
    public DateTime ArrivalTime { get; set; }
}