using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace H24.Definitions
{
    /// <summary>
    /// JSON-serializable user
    /// </summary>
    public class User
    {
        /// <summary>
        /// Holds the Id, which for our purposes is the username (and email)
        /// </summary>
        public string UserName { get; set; }
        
        public string PasswordHash { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public IEnumerable<string> Claims { get; }

        [JsonIgnore]
        public Guid CurrentSessionGuid { get; set; }

        [JsonIgnore]
        public bool IsAdmin => UserName != null && UserName.Equals("gus.gran@gmail.com");
        
        // User stats-based items.
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public int LoginCount { get; set; }

        /// <summary>
        /// Updates this user (likely deserialized from the network) from the given user.(likely from our DB)
        /// </summary>
        /// <param name="dbUser">The db user</param>
        internal void UpdateFromUser(User dbUser)
        {
            Name = dbUser.Name;
            RegistrationDate = dbUser.RegistrationDate;
            LastLoginDate = dbUser.LastLoginDate;
            LoginCount = dbUser.LoginCount;
        }
    }
}
