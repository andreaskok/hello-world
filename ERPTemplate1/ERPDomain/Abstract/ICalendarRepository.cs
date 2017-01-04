using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface ICalendarRepository
    {
        IEnumerable<Calendar> Calendar { get; }
        IEnumerable<Calendar> GetCalendarPaging(Int64 startID, Int64 endID);
        IEnumerable<Calendar> CalendarWildSearch(string fieldName, string fieldValue);
        IEnumerable<Calendar> GetCalendarByID(Int64 id);
        void SaveCalendar(Calendar calendar);
        void DeleteCalendar(Calendar calendar);
        Int64 CalendarCount();
        Int64 GetMaxID();

    }
}
