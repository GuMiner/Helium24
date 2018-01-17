namespace Helium24.Models
{
    /// <summary>
    /// Holds a lot of booleans representing possible registration errors.
    /// </summary>
    public class LoginModel
    {
        public LoginModel()
        {
            MissingUser = false;
            BadPassword = false;
            BadEmail = false;
            MissingField = false;
        }
        
        public bool MissingUser { get; set; }
        public bool BadEmail { get; set; }
        public bool BadPassword { get; set; }
        public bool MissingField { get; set; }
    }
}
