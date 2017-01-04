using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Common.Migrations
{
    [Migration(201607150001)]
    public class _201607150001_Create_DB : Migration
    {
        public override void Up()
        {
            //Create.Schema("ERPDomain3");
        }

        public override void Down()
        {
            throw new NotImplementedException();
        }
    }
}