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
    public class EFChartOfAccount_DimensionValueRepository : IChartOfAccount_DimensionValueRepository
    {
        private EFDbContextGL context = new EFDbContextGL();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextGL"].ConnectionString);

        public IEnumerable<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValue
        {
            get { return context.ChartOfAccount_DimensionValue; }
        }

        public IEnumerable<ChartOfAccount_DimensionValue> ChartOfAccount_DimensionValueWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_DimensionValue>("SELECT * FROM ChartOfAccount_DimensionValue WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<ChartOfAccount_DimensionValue> GetChartOfAccount_DimensionValue(String fieldName, Int64 fieldValue)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_DimensionValue>("SELECT * FROM ChartOfAccount_DimensionValue WHERE " + fieldName + " = '" + fieldValue + "'");

        }

        public IEnumerable<ChartOfAccount_DimensionValue> GetChartOfAccount_DimensionValueByID(Int64 id)
        {
            //improve performance
            return cnn.Query<ChartOfAccount_DimensionValue>("SELECT * FROM ChartOfAccount_DimensionValue WHERE ID = " + id);

        }

        public void SaveChartOfAccount_DimensionValue(ChartOfAccount_DimensionValue chartofaccount_dimensionvalue)
        {
            if (chartofaccount_dimensionvalue.ID == 0)
            {
                context.ChartOfAccount_DimensionValue.Add(chartofaccount_dimensionvalue);
            }
            else
            {
                context.Entry(chartofaccount_dimensionvalue).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteChartOfAccount_DimensionValue(ChartOfAccount_DimensionValue chartofaccount_dimensionvalue)
        {
            context.ChartOfAccount_DimensionValue.Remove(chartofaccount_dimensionvalue);
            context.SaveChanges();
        }
    }
}
