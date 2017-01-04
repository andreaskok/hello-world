using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IDimension_SettingRepository
    {
        IEnumerable<Dimension_Setting> Dimension_Setting { get; }
        IEnumerable<Dimension_Setting> GetDimension_SettingPaging(Int64 startID, Int64 endID);
        IEnumerable<Dimension_Setting> Dimension_SettingWildSearch(string fieldName, string fieldValue);
        IEnumerable<Dimension_Setting> GetDimension_SettingByID(Int64 id);
        void SaveDimension_Setting(Dimension_Setting dimensionsetting);
        void DeleteDimension_Setting(Dimension_Setting dimensionsetting);
        long GetMaxID();
    }
}
