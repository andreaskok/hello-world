using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFActivityRepository : IActivityRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        public IEnumerable<Activity> Activity
        {
            get { return context.Activity; }
        }

        public void SaveActivity(Activity activity)
        {
            if (activity.ID == 0)
            {
                context.Activity.Add(activity);
            }
            else
            {
                context.Entry(activity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteActivity(Activity activity)
        {
            context.Activity.Remove(activity);
            context.SaveChanges();
        }

    }
}
