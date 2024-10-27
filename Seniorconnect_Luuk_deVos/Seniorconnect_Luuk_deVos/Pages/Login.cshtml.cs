using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Seniorconnect_Luuk_deVos.DAL;
using Seniorconnect_Luuk_deVos.Model;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Seniorconnect_Luuk_deVos.Pages
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        [BindProperty]
        public Credential Credential { get; set; }

        private readonly UserLogic _userLogic;

        public LoginModel(UserLogic userLogic)
        {
            _userLogic = userLogic;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var authResult = _userLogic.AuthenticateUser(Credential.email, Credential.password);

            if (authResult ==null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return Page();
            }


            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, authResult.userId.ToString()),
        new Claim(ClaimTypes.Email, authResult.email),
        new Claim(ClaimTypes.Name, authResult.name)
    };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return RedirectToPage("/Index"); // Redirect to a secure page after successful login
        }
    }

    public class Credential
    {
        [Required]
        public string email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }
}
