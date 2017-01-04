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
    public class EFDimensionSetupRepository : IDimensionSetupRepository
    {
        private EFDbContext context = new EFDbContext();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString);

        public IEnumerable<DimensionSetup> DimensionSetup
        {
            get { return context.DimensionSetup; }
        }

        public IEnumerable<DimensionSetup> DimensionSetupWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<DimensionSetup>("SELECT * FROM DimensionSetup WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public void SaveDimensionSetup(DimensionSetup dimensionsetup)
        {
            if (dimensionsetup.ID == 0)
            {
                context.DimensionSetup.Add(dimensionsetup);
            }
            else
            {
                context.Entry(dimensionsetup).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteDimensionSetup(DimensionSetup dimensionsetup)
        {
            context.DimensionSetup.Remove(dimensionsetup);
            context.SaveChanges();
        }

    }
}
