using System;
using System.Collections.Generic;
using System.Data.Entity;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;


namespace ERPDomain.Concrete
{
    public class EFDimension_SettingRepository : IDimension_SettingRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        //public IEnumerable<Dimension_Setting> Dimension_Setting
        //{
        //    get { return context.Dimension_Setting; }
        //}
        public IEnumerable<Dimension_Setting> Dimension_Setting
        {
            get
            {
                //improve performance
                return cnn.Query<Dimension_Setting>("SELECT * FROM Dimension_Setting");
            }
        }

        public IEnumerable<Dimension_Setting> GetDimension_SettingPaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Dimension_Setting>("SELECT * FROM Dimension_Setting WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Dimension_Setting> Dimension_SettingWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Dimension_Setting>("SELECT * FROM Dimension_Setting WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Dimension_Setting> GetDimension_SettingByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Dimension_Setting>("SELECT * FROM Dimension_Setting WHERE ID = " + id);

        }

        public void SaveDimension_Setting(Dimension_Setting dimensionsetting)
        {
            if (dimensionsetting.ID == 0)
            {
                context.Dimension_Setting.Add(dimensionsetting);
            }
            else
            {
                context.Entry(dimensionsetting).State = EntityState.Modified;
            }
            context.SaveChanges();
        }


        public void DeleteDimension_Setting(Dimension_Setting dimensionsetting)
        {
            context.Dimension_Setting.Attach(dimensionsetting);
            context.Dimension_Setting.Remove(dimensionsetting);
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

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Dimension_Setting";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
