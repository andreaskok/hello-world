using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using ERPDomain.Abstract;
using ERPDomain.Entities;
using Dapper;

namespace ERPDomain.Concrete
{
    public class EFLanguageRepository : ILanguageRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);
        public IEnumerable<Language> Language
        {
            get { return context.Language; }
        }

        public IEnumerable<Language> GetLanguagePaging(Int64 startID, Int64 endID)
        {
            //improve performance
            return cnn.Query<Language>("SELECT * FROM Language WHERE ID BETWEEN " + startID + " AND " + endID);

        }

        public IEnumerable<Language> LanguageWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Language>("SELECT * FROM Language WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");

        }

        public IEnumerable<Language> GetLanguageByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Language>("SELECT * FROM Language WHERE ID = " + id);

        }

        public void SaveLanguage(Language language)
        {
            if (language.ID == 0)
            {
                context.Language.Add(language);
            }
            else
            {
                context.Entry(language).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteLanguage(Language language)
        {
            context.Language.Attach(language);
            context.Language.Remove(language);
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

        public Int64 LanguageCount()
        {
            Int64 iCnt = 0;
            String sSql = "SELECT COUNT(*) FROM Language";
            iCnt = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iCnt;
        }

        public Int64 GetMaxID()
        {
            Int64 iMax = 0;
            String sSql = "SELECT Max(ID) FROM Language";
            iMax = context.Database.SqlQuery<Int64>(sSql).Single();
            //context.Database.Query
            return iMax;
        }

    }
}
