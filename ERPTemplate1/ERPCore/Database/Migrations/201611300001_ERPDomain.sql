ALTER TABLE dbo.sh_roleaccess ADD [DynamicMenuID] INT  NOT NULL  DEFAULT(0)
ALTER TABLE dbo.DynamicMenu ADD [ResourceName] NVARCHAR(100)  DEFAULT('')