﻿GO
/****** Object:  Table [dbo].[DfItemType]    Script Date: 11/1/2016 3:17:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfItemType](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](50) NOT NULL CONSTRAINT [DF_DfItem_Code]  DEFAULT (''),
	[Name] [nvarchar](100) NOT NULL CONSTRAINT [DF_DfItem_Name]  DEFAULT (''),
 CONSTRAINT [PK_DfItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


ALTER TABLE dbo.DfMasterData ADD [DfItemTypeID] BIGINT  NOT NULL  DEFAULT(0)
ALTER TABLE dbo.DfMaster ADD [CountryID] BIGINT  NOT NULL  DEFAULT(0)