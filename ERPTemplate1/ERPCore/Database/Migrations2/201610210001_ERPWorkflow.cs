using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Database.Migrations2
{
    [Migration(201610210001)]
    public class _201610210001_ERPWorkflow : Migration
    {
        
        public override void Up()
        {
            try
            {
                Console.WriteLine("path=" + AppDomain.CurrentDomain.BaseDirectory);
                IfDatabase("SqlServer")
                    .Execute.Script(AppDomain.CurrentDomain.BaseDirectory + "\\Database\\Migrations2\\201610210001_ERPWorkflow.sql");
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