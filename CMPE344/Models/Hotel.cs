using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CMPE344.Models;

public class Hotel
{
    [Key]
    public int HotelId { get; set; }

    [Required]
    [DisplayName("Name")]
    [StringLength(255)]
    public string HotelName { get; set; } = string.Empty;   

    [Required]
    [DisplayName("Location")]
    [StringLength(255)]
    public string Location { get; set; } = string.Empty;
}