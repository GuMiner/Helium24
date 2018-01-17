using Helium24.Definitions;
using Helium24.Models;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using System.Linq;
using System.Net.Mail;

namespace Helium24.Modules
{
    /// <summary>
    /// Handles secure login and registration API.
    /// </summary>
    public class SecureModule : NancyModule
    {
        public SecureModule()
            : base("/Secure")
        {
            Get["/Login"] = parameters =>
            {
                return View["Login", new LoginModel()];
            };

            Get["/Register"] = parameters =>
            {
                return View["Register", new RegisterModel()];
            };

            Post["/Login"] = parameters =>
            {
                User user = this.Bind<User>();

                LoginModel invalidLoginModel = null;

                if (string.IsNullOrWhiteSpace(user.UserName) || string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    invalidLoginModel = new LoginModel();
                    invalidLoginModel.MissingField = true;
                }
                else
                {
                    try
                    {
                        new MailAddress(user.UserName);
                    }
                    catch
                    {
                        invalidLoginModel = new LoginModel();
                        invalidLoginModel.BadEmail = true;
                    }
                }

                if (invalidLoginModel == null)
                {
                    bool userNotFound, invalidPassword;
                    if (!UserMapper.Login(user, out userNotFound, out invalidPassword))
                    {
                        if (userNotFound)
                        {
                            invalidLoginModel = new LoginModel();
                            invalidLoginModel.MissingUser = true;
                        }
                        else if (invalidPassword)
                        {
                            invalidLoginModel = new LoginModel();
                            invalidLoginModel.BadPassword = true;
                        }

                        return View["Login", invalidLoginModel];
                    }

                    return this.LoginAndRedirect(user.CurrentSessionGuid, null, "/Secure");
                }
                else
                {
                    return View["Login", invalidLoginModel];
                }
            };

            Post["/Logout"] = parameters =>
            {
                if (this.Context.CurrentUser != null && this.Context.CurrentUser is User)
                {
                    UserMapper.Logout((this.Context.CurrentUser as User).CurrentSessionGuid);
                }

                return this.LogoutAndRedirect("/Secure");
            };

            // Receives the registration request and attempts to parse out the data in a valid manner.
            Post["/Register"] = parameters =>
            {
                User user = this.Bind<User>();

                // Validate 
                RegisterModel invalidRegistrationModel = null;
                if (string.IsNullOrWhiteSpace(user.UserName) ||
                    string.IsNullOrWhiteSpace(user.Name) ||
                    string.IsNullOrWhiteSpace(user.PasswordHash))
                {
                    invalidRegistrationModel = new RegisterModel();
                    invalidRegistrationModel.MissingField = true;
                }
                else if (user.Name.Split(new char[] { ' ' }).Any(item => item.Length < 2))
                {
                    invalidRegistrationModel = new RegisterModel();
                    invalidRegistrationModel.BadName = true;
                }
                else
                {
                    try
                    {
                        new MailAddress(user.UserName);
                    }
                    catch
                    {
                        invalidRegistrationModel = new RegisterModel();
                        invalidRegistrationModel.BadEmail = true;
                    }
                }

                if (invalidRegistrationModel == null)
                {
                    // Perform registration.
                    if (!UserMapper.RegisterUser(user))
                    {
                        invalidRegistrationModel = new RegisterModel();
                        invalidRegistrationModel.ExistingUser = true;
                        return View["Register", invalidRegistrationModel];
                    }
                    
                    return this.LoginAndRedirect(user.CurrentSessionGuid, null, "/Secure");
                }
                else
                {
                    return View["Register", invalidRegistrationModel];
                }
            };
        }
    }
}
