using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMPE344.Models;

public class Tour
{
    public int TourId { get; set; }

    [Required]
    [DisplayName("Title")]
    [StringLength(45)]
    public string Title { get; set; } = string.Empty;

    [DisplayName("Description")]
    [StringLength(1023)]
    public string? Description { get; set; }

    [Required]
    [DisplayName("Start")]
    public DateTime StartDate { get; set; }

    public string StartDateString => StartDate.ToString("g");

    [Required]
    [DisplayName("End")]
    public DateTime EndDate { get; set; }

    public string EndDateString => EndDate.ToString("g");

    [Required]
    [DisplayName("Capacity")]
    [Range(0, int.MaxValue)]
    public int Capacity { get; set; }

    [Required]
    [DisplayName("Price")]
    public double Price { get; set; }

    public int HotelId { get; set; }

    public int FlightId { get; set; }

    public int CreatedBy { get; set; }

    public int Applied { get; set; }

    [DisplayName("Quota")]
    public int RemainingQuota => Capacity - Applied;
}