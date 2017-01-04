using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Database.Migrations
{
    [Migration(201611020001)]
    public class _201611020001_ERPDomain_V1 : Migration
    {
        public override void Up()
        {
            try
            {
                Console.WriteLine("path=" + AppDomain.CurrentDomain.BaseDirectory);
                IfDatabase("SqlServer")
                    .Execute.Script(AppDomain.CurrentDomain.BaseDirectory + "\\Database\\Migrations\\201611020001_ERPDomain_V1.sql");
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex=" + ex.Message);
            }

        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}
