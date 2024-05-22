using System.ComponentModel.DataAnnotations;

namespace CMPE344.ViewModels.Account;

public class RegisterViewModel
{
    [Required(AllowEmptyStrings = false)]
    [Display(Name = "First Name")]
    [StringLength(maximumLength: 255)]
    public string FirstName { get; set; } = string.Empty;


    [Required(AllowEmptyStrings = false)]
    [Display(Name = "Last Name")]
    [StringLength(maximumLength: 255)]
    public string LastName { get; set; } = string.Empty;


    [Required(AllowEmptyStrings = false)]
    [EmailAddress]
    [Display(Name = "Email")]
    [StringLength(maximumLength: 255)]
    public string Email { get; set; } = string.Empty;


    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    [StringLength(maximumLength: 255, MinimumLength = 5)]
    public string Password { get; set; } = string.Empty;


    [Required(AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [StringLength(maximumLength: 255)]
    public string ConfirmPassword { get; set; } = string.Empty;


    [Display(Name = "Address")]
    [StringLength(maximumLength: 255)]
    public string? Address { get; set; }


    [Phone]
    [Display(Name = "Phone number")]
    [StringLength(maximumLength: 255)]
    public string? PhoneNumber { get; set; }


    [Required]
    [Display(Name = "User Type")]
    [AllowedValues("Customer", "Travel Agent")]
    public string UserType { get; set; } = "Customer";

    [Required]
    [Display(Name = "Agency Name")]
    [StringLength(maximumLength: 255)]
    public string AgencyName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Commission Rate")]
    [Range(0, double.MaxValue)]
    public double CommissionRate { get; set; }
}