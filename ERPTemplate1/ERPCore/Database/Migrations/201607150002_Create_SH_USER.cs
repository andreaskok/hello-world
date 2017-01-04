using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FluentMigrator;

namespace ERPCore.Common.Migrations
{
    [Migration(201607150002)]
    public class _201607150002_Create_SH_USER : Migration
    {
        public override void Up()
        {

            //Create.Table("SH_USER")
            //    .WithColumn("ID").AsInt32().NotNullable().PrimaryKey().Identity()
            //    .WithColumn("UserID").AsString(50).NotNullable().WithDefaultValue("")
            //    .WithColumn("SH_ROLEID").AsInt32().NotNullable().WithDefaultValue(0)
            //    .WithColumn("UserPwd").AsString(100).NotNullable().WithDefaultValue("")
            //    .WithColumn("UserName").AsString(200).NotNullable().WithDefaultValue("")
            //    .WithColumn("UserEmail").AsString(50).NotNullable().WithDefaultValue("")
            //    .WithColumn("CreateDate").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now)
            //    .WithColumn("UpdateDate").AsDateTime().NotNullable().WithDefaultValue(DateTime.Now);

            //Create.Index("ix_UserName").OnTable("SH_USER").OnColumn("UserName").Ascending()
            //    .WithOptions().NonClustered();

            //Insert.IntoTable("SH_USER").Row(new
            //{
            //    UserID = "user1",
            //    SH_ROLEID = 6,
            //    UserPwd = "user123",
            //    UserName = "User 1",
            //    UserEmail = "user1@hotmail.com",
            //    CreateDate = DateTime.Now,
            //    UpdateDate = DateTime.Now
            //});

            Insert.IntoTable("SH_USER").Row(new
            {
                UserID = "user2",
                SH_ROLEID = 0,
                UserPwd = "user123",
                UserName = "User 2",
                UserEmail = "user2@hotmail.com",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });

            Insert.IntoTable("SH_USER").Row(new
            {
                UserID = "user3",
                SH_ROLEID = 0,
                UserPwd = "user123",
                UserName = "User 3",
                UserEmail = "user3@hotmail.com",
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now
            });
        }

        public override void Down()
        {
            //Delete.Table("SH_USER");
        }
    }
}