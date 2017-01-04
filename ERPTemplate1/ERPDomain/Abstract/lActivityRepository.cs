using System.Collections.Generic;
using ERPDomain.Entities;
using System;

namespace ERPDomain.Abstract
{
    public interface IActivityRepository
    {
        IEnumerable<Activity> Activity { get; }
        void SaveActivity(Activity activity);
        void DeleteActivity(Activity activity);
    }
}
