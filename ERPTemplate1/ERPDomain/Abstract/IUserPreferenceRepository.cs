using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IUserPreferenceRepository
    {
        IEnumerable<UserPreference> UserPreference { get; }
        IEnumerable<UserPreference> GetUserPreference(String fieldName, Int64 fieldValue);
        IEnumerable<UserPreference> GetUserPreferencePaging(Int64 startID, Int64 endID);
        IEnumerable<UserPreference> UserPreferenceWildSearch(string fieldName, string fieldValue);
        IEnumerable<UserPreference> GetUserPreferenceByID(Int64 id);
        IEnumerable<UserPreference> GetUserPreferenceBySH_USERID(Int64 id);        
        void SaveUserPreference(UserPreference userpreference);
        void DeleteUserPreference(UserPreference userpreference);
        Int64 UserPreferenceCount();
        Int64 GetMaxID();
    }
}
