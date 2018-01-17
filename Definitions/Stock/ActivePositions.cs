using System.Collections.Generic;

namespace Helium24.Definitions
{
    /// <summary>
    /// JSON-serializable active positions
    /// </summary>
    public class ActivePositions
    {
        /// <summary>
        /// Holds the Id, which for our purposes is the username who owns the item
        /// </summary>
        public string Id { get; set; }

        public List<ActiveStock> Positions { get; set; }
    }
}
