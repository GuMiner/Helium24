using System;

namespace Helium24.Models
{
    /// <summary>
    /// Represents a physical cached document accessible in my library.
    /// </summary>
    public class PiCachedDocument
    {
        public String Title { get; set; }
        public String Source { get; set; }
        public String Format { get; set; }
        
        public String Category { get; set; }
        public String Association { get; set; }

        public String License { get; set; }

        public bool IsPublicallyAvailable { get; set; }
    }
}