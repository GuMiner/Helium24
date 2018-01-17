using System;

namespace Helium24.Definitions
{
    /// <summary>
    /// JSON-serializable Note
    /// </summary>
    public class Note
    {
        /// <summary>
        /// Holds the Id, which for our purposes is the username who owns the note.
        /// </summary>
        public string Id { get; set; }
        public string Notes { get; set; }
        public string LastNotes { get; set; }
        public DateTime LastEdit { get; set; }
    }
}
