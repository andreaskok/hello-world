using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Common.Migrations
{
    [Migration(201607150003)]
    public class _201607150003_Create_ParentMenu : Migration
    {
        public override void Up()
        {
            //Create.Table("ParentMenu")
            //    .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            //    .WithColumn("Name").AsString(50).NotNullable().WithDefaultValue("")
            //    .WithColumn("Area").AsString(100).NotNullable().WithDefaultValue("")
            //    .WithColumn("ControllerName").AsString(100).NotNullable().WithDefaultValue("")
            //    .WithColumn("MethodName").AsString(100).NotNullable().WithDefaultValue("")
            //    .WithColumn("PlugIn").AsBinary().NotNullable().WithDefaultValue(0)
            //    .WithColumn("Buy").AsBinary().NotNullable().WithDefaultValue(0);

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "Admin",
            //    Area = "",
            //    ControllerName = "SH_USER",
            //    MethodName = "Index",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "Budget2DB",
            //    Area = "",
            //    ControllerName = "Budget",
            //    MethodName = "List",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "AccountingExt",
            //    Area = "ERPAccounting",
            //    ControllerName = "BillParty",
            //    MethodName = "List",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "AccountingExt",
            //    Area = "ERPAccounting",
            //    ControllerName = "BillParty",
            //    MethodName = "List",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "Finance",
            //    Area = "Finance Account",
            //    ControllerName = "Account",
            //    MethodName = "List",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "General Ledger",
            //    Area = "",
            //    ControllerName = "AccountGroup",
            //    MethodName = "Index",
            //    Plugin = 0,
            //    Buy = 0
            //});

            //Insert.IntoTable("ParentMenu").Row(new
            //{
            //    Name = "System",
            //    Area = "",
            //    ControllerName = "PluginArea",
            //    MethodName = "Index",
            //    Plugin = 0,
            //    Buy = 0
            //});
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}