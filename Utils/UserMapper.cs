using Helium24.Definitions;
using Nancy.Authentication.Forms;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Helium24
{
    public class UserMapper : IUserMapper
    {
        // Listing of the logged-in users. Note that this supports multiple sessions of the same user (which are all logged-out upon logout)
        private static ConcurrentDictionary<Guid, User> loggedInUsers = new ConcurrentDictionary<Guid, User>();

        /// <summary>
        /// Returns the user given the specified ID. Only looks at currently logged-in users.
        /// </summary>
        public IUserIdentity GetUserFromIdentifier(Guid identifier, NancyContext context)
        {
            User user;
            if (loggedInUsers.TryGetValue(identifier, out user))
            {
                return user;
            }

            return null;
        }
        
        /// <summary>
        /// Registers the new user, returning false if the user is already registered.
        /// Also populates the user's current session ID (effectively logging the user in).
        /// </summary>
        /// <param name="user">The user to register.</param>
        /// <returns>True on success, false if the user is already registered.</returns>
        internal static bool RegisterUser(User user)
        {
            user.RegistrationDate = DateTime.UtcNow;
            user.LastLoginDate = DateTime.UtcNow;
            user.LoginCount = 1;
            user.EnabledFeatures = new HashSet<string>();

            try
            {
                User existingUser = Global.UserStore.GetUser(user.UserName);
                if (existingUser != null)
                {
                    Global.Log($"User already exists: {user.UserName}.");
                    return false;
                }

                Global.UserStore.SaveUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Registration failure: {ex.Message}");
                return false;
            }

            user.CurrentSessionGuid = Guid.NewGuid();
            loggedInUsers.TryAdd(user.CurrentSessionGuid, user);
            LogUserCount(loggedInUsers.Count);
            return true;
        }

        /// <summary>
        /// Logs in the specified user.
        /// </summary>
        internal static bool Login(User user, out bool userNotFound, out bool invalidPassword)
        {
            userNotFound = false;
            invalidPassword = false;

            try
            {
                User existingUser = Global.UserStore.GetUser(user.UserName);
                if (existingUser == null)
                {
                    Console.WriteLine($"Invalid login for user: {user.UserName}.");
                    userNotFound = true;
                    return false;
                }
                else if (!existingUser.PasswordHash.Equals(user.PasswordHash))
                {
                    Console.WriteLine($"Invalid password for user: {user.UserName}.");
                    invalidPassword = true;
                    return false;
                }

                // Valid response, update the user's last login date and login count.
                user.UpdateFromUser(existingUser);
                user.LastLoginDate = DateTime.UtcNow;
                user.LoginCount++;

                Global.UserStore.UpdateUserLoginData(user.UserName, user.LastLoginDate, user.LoginCount);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DB failure!: {ex.Message}");
                userNotFound = true;
                return false;
            }

            user.CurrentSessionGuid = Guid.NewGuid();
            loggedInUsers.TryAdd(user.CurrentSessionGuid, user);
            LogUserCount(loggedInUsers.Count);
            return true;
        }

        /// <summary>
        /// Remove the user from the list of currently logged-in users.
        /// </summary>
        /// <param name="userId">The user to logout</param>
        internal static void Logout(Guid userId)
        {
            User removedUser;
            if (loggedInUsers.TryRemove(userId, out removedUser))
            {
                // Remove outdated users.
                IEnumerable<Guid> priorUserSessions = loggedInUsers.Where(aUser => aUser.Value.UserName.Equals(removedUser.UserName)).Select(pair => pair.Key);
                foreach (Guid oldUser in priorUserSessions)
                {
                    loggedInUsers.TryRemove(oldUser, out removedUser);
                }
            }


            LogUserCount(loggedInUsers.Count);
        }

        private static void LogUserCount(int userCount)
        {
            Console.WriteLine($"Currently there {(loggedInUsers.Count == 1 ? "is" : "are")} {userCount} users logged-in.");
        }
    }
}
