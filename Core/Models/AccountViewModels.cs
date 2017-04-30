using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MyEShop.Core.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(15, ErrorMessage = "First name must be less than 20 characters.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(15, ErrorMessage = "Last name must be less than 30 characters.")]
        public string LastName { get; set; }

        [Display(Name = "Profile picture")]
        public byte[] ProfilePicture { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "Address must be 30 characters or shorter.")]
        public string Address { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "City must be 30 characters or shorter.")]
        public string City { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "State must be 10 characters or shorter.")]
        public string State { get; set; }

        [Required]
        [StringLength(30, ErrorMessage = "ZipCode must be 15 characters or shorter.")]
        public string ZipCode { get; set; }

        [StringLength(30, ErrorMessage = "Mobile must be 30 characters or shorter.")]
        public string Mobile { get; set; }

        [StringLength(70, ErrorMessage = "Bank name must be 70 characters or shorter.")]
        public string BankName { get; set; }

        [StringLength(20, ErrorMessage = "Account must be 20 characters or shorter.")]
        public string BankAccount { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
