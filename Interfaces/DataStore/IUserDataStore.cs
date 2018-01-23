using Helium24.Definitions;
using System;

namespace Helium24.Interfaces
{
    public interface IUserDataStore
    {
        User GetUser(string userName);
        void SaveUser(User user);
        void UpdateUserLoginData(string userName, DateTime lastLoginDate, int loginCount);
    }
}
