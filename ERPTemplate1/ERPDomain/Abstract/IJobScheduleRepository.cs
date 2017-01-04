using System;
using System.Collections.Generic;
using ERPDomain.Entities;

namespace ERPDomain.Abstract
{
    public interface IJobScheduleRepository
    {
        IEnumerable<JobSchedule> JobSchedule { get; }
        IEnumerable<JobSchedule> GetJobSchedulePaging(Int64 startID, Int64 endID);
        IEnumerable<JobSchedule> JobScheduleWildSearch(string fieldName, string fieldValue);
        IEnumerable<JobSchedule> GetJobScheduleByID(Int64 id);
        void SaveJobSchedule(JobSchedule jobschedule);
        void DeleteJobSchedule(JobSchedule jobschedule);

        Int64 JobScheduleCount();
        Int64? GetMaxID();
    }
}
