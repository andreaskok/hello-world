using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Database.Migrations
{
    [Migration(201609300003)]
    public class _201609300003_ERPDomain_V3 : Migration
    {
        public override void Up()
        {
            try
            {
                Console.WriteLine("path=" + AppDomain.CurrentDomain.BaseDirectory);
                IfDatabase("SqlServer")
                    .Execute.Script(AppDomain.CurrentDomain.BaseDirectory + "\\Database\\Migrations\\201609300003_ERPDomain_V3.sql");
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