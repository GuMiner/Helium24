using Helium24.Definitions;
using System;
using System.Collections.Generic;

namespace Helium24.Interfaces
{
    public interface IUserDataStore
    {
        User GetUser(string userName);
        void SaveUser(User user);
        void UpdateUserEnabledFeatures(string userName, HashSet<string> enabledFeatures);
        void UpdateUserLoginData(string userName, DateTime lastLoginDate, int loginCount);
    }
}
