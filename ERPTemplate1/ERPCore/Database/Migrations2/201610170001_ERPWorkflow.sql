insert into dbo.WfStateType (Name) values ('Complete')
insert into dbo.WfState (WfStateTypeID,WfProcessID,Name) values ('7','1','Complete Payment')
insert into dbo.WfTransition (WfProcessID,CurrentStateID, NextStateID) values ('1','6','7')
update dbo.WfAction SET WfActionTypeID = 4 Where ID = 4
update dbo.WfAction SET WfActionTypeID = 5 Where ID = 5
update dbo.WfAction SET WfActionTypeID = 6 Where ID = 6
insert into dbo.WfActionType (Name) values ('Complete')
insert into dbo.WfAction (WfActionTypeID,WfProcessID,Name) values ('11','1','Complete Payment')
ALTER TABLE dbo.WfRequestAction ADD
	UserID int NOT NULL CONSTRAINT DF_WfRequestAction_UserID DEFAULT 0,
	CreateDate datetime2(7) NOT NULL CONSTRAINT DF_WfRequestAction_CreateDate DEFAULT getdate(),
	UpdateDate datetime2(7) NOT NULL CONSTRAINT DF_WfRequestAction_UpdateDate DEFAULT getdate()
insert into dbo.WfTransition (WfProcessID,CurrentStateID,NextStateID) values ('1','5','7')
insert into dbo.WfTransition (WfProcessID, CurrentStateID, NextStateID) values ('1','5','3')