using System.ComponentModel.DataAnnotations;

namespace CMPE344.ViewModels.Account;

public class ProfileViewModel
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


    [Display(Name = "Address")]
    [StringLength(maximumLength: 255)]
    public string? Address { get; set; }


    [Phone]
    [Display(Name = "Phone number")]
    [StringLength(maximumLength: 255)]
    public string? PhoneNumber { get; set; }


    public bool IsCustomer { get; set; }

    #region Customer Only

    [Required]
    [Display(Name = "Email")]
    public bool EmailPreference { get; set; } = true;

    [Required]
    [Display(Name = "Phone")]
    public bool PhonePreference { get; set; } = true;

    #endregion

    #region Travel Agent Only

    [Required]
    [Display(Name = "Agency Name")]
    [StringLength(maximumLength: 255)]
    public string AgencyName { get; set; } = string.Empty;

    [Required]
    [Display(Name = "Commission Rate")]
    [Range(0, double.MaxValue)]
    public double CommissionRate { get; set; }

    #endregion


    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Original password")]
    [StringLength(maximumLength: 255, MinimumLength = 5)]
    public string OriginalPassword { get; set; } = string.Empty;


    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "New password")]
    [StringLength(maximumLength: 255, MinimumLength = 5)]
    public string NewPassword { get; set; } = string.Empty;


    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm password")]
    [Compare(nameof(NewPassword), ErrorMessage = "The password and confirmation password do not match.")]
    [StringLength(maximumLength: 255)]
    public string ConfirmNewPassword { get; set; } = string.Empty;
}