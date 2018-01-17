namespace Helium24.Models
{
    /// <summary>
    /// Determines what data to show on the secure page.
    /// </summary>
    public class SecureModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }

        public int VisitCount { get; set; }
    }
}
