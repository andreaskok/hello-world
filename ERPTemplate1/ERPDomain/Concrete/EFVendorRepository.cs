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
    public class EFVendorRepository : IVendorRepository
    {
        private EFDbContextAP context = new EFDbContextAP();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAP"].ConnectionString);

        public IEnumerable<Vendor> Vendor
        {
            get { return context.Vendor; }
        }

        public IEnumerable<Vendor> VendorWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Vendor>("SELECT * FROM Vendor WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public void SaveVendor(Vendor vendor)
        {
            if (vendor.ID == 0)
            {
                context.Vendor.Add(vendor);
            }
            else
            {
                context.Entry(vendor).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteVendor(Vendor vendor)
        {
            context.Vendor.Remove(vendor);
            context.SaveChanges();
        }

    }

}
