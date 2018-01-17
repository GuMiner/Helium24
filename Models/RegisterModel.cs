namespace Helium24.Models
{
    /// <summary>
    /// Holds a lot of booleans representing possible registration errors.
    /// </summary>
    public class RegisterModel
    {
        public RegisterModel()
        {
            ExistingUser = false;
            BadEmail = false;
            BadName = false;
            MissingField = false;
        }
        
        public bool ExistingUser { get; set; }
        public bool BadEmail { get; set; }
        public bool BadName { get; set; }
        public bool MissingField { get; set; }
    }
}
