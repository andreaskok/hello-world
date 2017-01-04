update dbo.DfMaster SET COUNTRYID = '150' WHERE ID = 1
insert into dbo.DfMaster (name,purpose,countryId) values ('VAT','VAT Indonesia','57')

insert into dbo.DfItemType (code,name) values ('A1','Computer')
insert into dbo.DfItemType (code,name) values ('A2','Furniture')
insert into dbo.DfItemType (code,name) values ('A3','Stationery')

insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','VatPct','16','1')
insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','HeaderRounding','0','1')
insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','LineRounding','0','1')
insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','VatPct','10','2')
insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','HeaderRounding','0','2')
insert into dbo.DfMasterData (DfMasterID,Name,Value,DfItemTypeID) values ('2','LineRounding','0','2')