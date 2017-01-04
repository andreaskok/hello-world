insert into dbo.DynamicMenu (ParentID,MenuLevel,MenuName,ControllerName,MethodName,AreaName,PluginName,Plugin,Buy,CreateDate,UpdateDate) values
('9','2','Workflow Inbox','Payment','WorkflowInboxIndex','PluginAP','','1','1',getdate(),getdate())
ALTER TABLE dbo.Payment ADD UserID int NOT NULL CONSTRAINT DF_Payment_UserID DEFAULT 0