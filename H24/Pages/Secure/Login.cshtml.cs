using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using H24.Definitions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace H24.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<LoginModel> logger;

        public bool MissingUser { get; private set; }

        public bool BadEmail { get; private set; }

        public bool BadPassword { get; private set; }

        public bool MissingField { get; private set; }

        [BindProperty]
        public string UserName { get; set; }

        [BindProperty]
        public string PasswordHash { get; set; }

        public LoginModel(ILogger<LoginModel> logger)
        {
            this.logger = logger;
        }

        public void Reset()
        {
            this.MissingUser = false;
            this.BadEmail = false;
            this.BadPassword = false;
            this.MissingField = false;
        }

        public void OnGet()
        {
            this.Reset();
        }

        /// <summary>
        /// Login
        /// </summary>
        public IActionResult OnPost()
        {
            this.Reset();

            User user = new User()
            {
                UserName = this.UserName,
                PasswordHash = this.PasswordHash
            };

            if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.PasswordHash))
            {
                this.MissingField = true;
                return Page();
            }
            else
            {
                try
                {
                    new MailAddress(user.UserName);
                }
                catch
                {
                    this.BadEmail = true;
                    return Page();
                }
            }

            bool userNotFound, invalidPassword;
            if (!this.Login(user, out userNotFound, out invalidPassword))
            {
                if (userNotFound)
                {
                    this.MissingUser = true;
                    return Page();
                }
                else if (invalidPassword)
                {
                    this.BadPassword = true;
                    return Page();
                }
            }

            ClaimsIdentity identity = new ClaimsIdentity(
                new[]
                {
                    new Claim(ClaimTypes.Email, user.UserName),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "Unknown"),
                },
                CookieAuthenticationDefaults.AuthenticationScheme);

            this.PageContext.HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity)).GetAwaiter().GetResult();
            return Redirect("/Secure/Secure");
        }

        /// <summary>
        /// Logs in the specified user.
        /// </summary>
        private bool Login(User user, out bool userNotFound, out bool invalidPassword)
        {
            userNotFound = false;
            invalidPassword = false;

            try
            {
                User existingUser = Program.UserStore.GetUser(user.UserName);
                if (existingUser == null)
                {
                    this.logger.LogData($"Invalid login for user: {user.UserName}.", this.HttpContext.TraceIdentifier);
                    userNotFound = true;
                    return false;
                }
                else if (!existingUser.PasswordHash.Equals(user.PasswordHash))
                {
                    this.logger.LogData($"Invalid password for user: {user.UserName}.", this.HttpContext.TraceIdentifier);
                    invalidPassword = true;
                    return false;
                }

                // Valid response, update the user's last login date and login count.
                user.UpdateFromUser(existingUser);
                user.LastLoginDate = DateTime.UtcNow;
                user.LoginCount++;

                Program.UserStore.UpdateUserLoginData(user.UserName, user.LastLoginDate, user.LoginCount);
            }
            catch (Exception ex)
            {
                this.logger.LogData($"DB failure!: {ex.Message}", this.HttpContext.TraceIdentifier);
                userNotFound = true;
                return false;
            }

            user.CurrentSessionGuid = Guid.NewGuid();
            return true;
        }
    }
}
