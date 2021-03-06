﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using ERPDomain.Abstract;
using ERPDomain.Entities;

namespace ERPDomain.Concrete
{
    public class EFWfStateActivityRepository : IWfStateActivityRepository
    {
        private EFDbContextWorkflow context = new EFDbContextWorkflow();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString);
        public IEnumerable<WfStateActivity> WfStateActivity
        {
            get
            {
                //improve performance
                return cnn.Query<WfStateActivity>("SELECT * FROM WfStateActivity");
            }
        }

        public void SaveWfStateActivity(WfStateActivity wfStateActivity)
        {
            if (wfStateActivity.ID == 0)
            {
                context.WfStateActivity.Add(wfStateActivity);
            }
            else
            {
                context.Entry(wfStateActivity).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteWfStateActivity(WfStateActivity wfStateActivity)
        {
            context.WfStateActivity.Attach(wfStateActivity);
            context.WfStateActivity.Remove(wfStateActivity);
            //context.SaveChanges();
            bool saveFailed;
            do
            {
                saveFailed = false;

                try
                {
                    context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    saveFailed = true;

                    // Update the values of the entity that failed to save from the store 
                    ex.Entries.Single().Reload();
                }

            } while (saveFailed);
            //https://msdn.microsoft.com/en-us/data/jj592904, fixed concurrency exception

        }
    }
}
