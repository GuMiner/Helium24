using H24.Definitions;
using System;

namespace H24.Definitions.DataStore
{
    public interface IUserDataStore
    {
        User GetUser(string userName);
        void SaveUser(User user);
        void UpdateUserLoginData(string userName, DateTime lastLoginDate, int loginCount);
    }
}
