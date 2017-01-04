using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Common.Migrations
{
    [Migration(201607150005)]
    public class _201607150005_Create_SH_ROLE : Migration
    {
        public override void Up()
        {
            //Create.Table("SH_ROLE")
            //    .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            //    .WithColumn("RoleName").AsString(100).NotNullable().WithDefaultValue("")
            //    .WithColumn("RoleDescription").AsString(200).NotNullable().WithDefaultValue("")
            //    .WithColumn("AdminFlag").AsBinary().NotNullable().WithDefaultValue(0)
            //    .WithColumn("CreateDate").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
            //    .WithColumn("UpdateDate").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);



        }

        public override void Down()
        {
            throw new NotImplementedException();
        }

    }
}