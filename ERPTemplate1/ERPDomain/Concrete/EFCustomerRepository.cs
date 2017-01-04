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
    public class EFCustomerRepository : ICustomerRepository
    {
        private EFDbContextAR context = new EFDbContextAR();
        private IDbConnection cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["EFDbContextAR"].ConnectionString);

        public IEnumerable<Customer> Customer
        {
            get { return context.Customer; }
        }

        public IEnumerable<Customer> CustomerWildSearch(String fieldName, String fieldValue)
        {
            //improve performance
            return cnn.Query<Customer>("SELECT * FROM Customer WHERE " + fieldName + " LIKE '%" + fieldValue + "%'");
        }

        public IEnumerable<Customer> GetCustomerByID(Int64 id)
        {
            //improve performance
            return cnn.Query<Customer>("SELECT * FROM Customer WHERE ID = " + id);

        }

        public void SaveCustomer(Customer customer)
        {
            if (customer.ID == 0)
            {
                context.Customer.Add(customer);
            }
            else
            {
                context.Entry(customer).State = EntityState.Modified;
            }
            context.SaveChanges();
        }

        public void DeleteCustomer(Customer customer)
        {
            context.Customer.Remove(customer);
            context.SaveChanges();
        }

    }

}
