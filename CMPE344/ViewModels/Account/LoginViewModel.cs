using System.ComponentModel.DataAnnotations;

namespace CMPE344.ViewModels.Account;

public class LoginViewModel
{
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
}