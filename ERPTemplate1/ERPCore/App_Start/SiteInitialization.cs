using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ERPCore.Models;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Initialization;

namespace ERPCore.App_Start
{
    public static partial class SiteInitialization
    {

        public static void ApplicationStart()
        {
            runMigrations();
            runMigrations2();
            //ModelBinders.Binders.Add(typeof(DateTime?), new DateTimeModelBinder());
        }

        private static void runMigrations()
        {
            string databaseType = "SqlServer";
            string cnnStr = ConfigurationManager.ConnectionStrings["EFDbContext"].ConnectionString;

            try
            {
                using (var sw = new StringWriter())
                {
                    var announcer = new TextWriterWithGoAnnouncer(sw)
                    {
                    };

                    var runner = new RunnerContext(announcer)
                    {
                        Database = databaseType,
                        Connection = cnnStr,
                        Targets = new string[] { typeof(SiteInitialization).Assembly.Location },
                        Task = "migrate",
                        WorkingDirectory = Path.GetDirectoryName(typeof(SiteInitialization).Assembly.Location),
                        Namespace = "ERPCore.Database.Migrations"
                    };
                    //Namespace = "ERPCore.Common.Migrations"
                    new TaskExecutor(runner).Execute();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ex=" + ex.Message);
            }
            
        }

        private static void runMigrations2()
        {
            string databaseType = "SqlServer";
            string cnnStr = ConfigurationManager.ConnectionStrings["EFDbContextWorkflow"].ConnectionString;

            try
            {
                using (var sw = new StringWriter())
                {
                    var announcer = new TextWriterWithGoAnnouncer(sw)
                    {
                    };

                    var runner = new RunnerContext(announcer)
                    {
                        Database = databaseType,
                        Connection = cnnStr,
                        Targets = new string[] { typeof(SiteInitialization).Assembly.Location },
                        Task = "migrate",
                        WorkingDirectory = Path.GetDirectoryName(typeof(SiteInitialization).Assembly.Location),
                        Namespace = "ERPCore.Database.Migrations2"
                    };
                    //Namespace = "ERPCore.Common.Migrations"
                    new TaskExecutor(runner).Execute();
                }
            }
            catch (Exception ex)
            {
                System.Console.WriteLine("Ex=" + ex.Message);
            }

        }

    }
}