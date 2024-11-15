using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seniorconnect_Luuk_deVos.DAL;
using System.ComponentModel.DataAnnotations;

namespace Seniorconnect_Luuk_deVos.Pages
{
    public class RegisterPageModel : PageModel
    {
        [BindProperty]
        public RegisterCredential Credential { get; set; }

        private readonly UserLogic _userLogic;

        public RegisterPageModel(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public IActionResult OnPost()
        {

            // Check if passwords match
            if (Credential.password != Credential.confirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            // Attempt to create the account
            var success = _userLogic.CreateAccount(Credential.email, Credential.password, Credential.name);

            if (!success)
            {
                ModelState.AddModelError(string.Empty, "Registration failed. Email might already be in use.");
                return Page();
            }

            return RedirectToPage("/Login");
        }
    }

    public class RegisterCredential
    {
        [Required]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string confirmPassword { get; set; }

        public string name { get; set; }
    }
}
