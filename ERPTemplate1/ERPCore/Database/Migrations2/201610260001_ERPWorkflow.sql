insert into DfMasterData (DfMasterID, Name, Value) values ('1','LineRounding','2')
insert into DfMasterData (DfMasterID, Name, Value) values ('1','HeaderRoundingMode','AwayFromZero')
insert into DfMasterData (DfMasterID, Name, Value) values ('1','LineRoundingMode','AwayFromZero')

update dfMasterData set name = 'HeaderRounding', value = '0' where ID=2
ALTER TABLE [DfRequestData] ADD [HeaderID] BIGINT  NOT NULL  DEFAULT(0)
ALTER TABLE [DfRequestData] ADD [LineID] BIGINT  NOT NULL  DEFAULT(0)
ALTER TABLE [DfRequestData] ADD [TableID] BIGINT  NOT NULL  DEFAULT(0)
CREATE NONCLUSTERED INDEX SearchIndex ON DfRequestData (HeaderID,LineID,TableID,Name)
CREATE NONCLUSTERED INDEX SearchIndex ON WfRequestData (Name) 

GO
/****** Object:  Table [dbo].[DfTable]    Script Date: 10/25/2016 5:16:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfTable](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL CONSTRAINT [DF_DfTableName_Name]  DEFAULT (''),
 CONSTRAINT [PK_DfTableName] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[DfTable] ON 

INSERT [dbo].[DfTable] ([ID], [Name]) VALUES (1, N'Payment')
INSERT [dbo].[DfTable] ([ID], [Name]) VALUES (2, N'PaymentLine')
SET IDENTITY_INSERT [dbo].[DfTable] OFF