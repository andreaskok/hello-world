GO
/****** Object:  Table [dbo].[DfExemptItem]    Script Date: 10/21/2016 2:22:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfExemptItem](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](100) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Exempt] [bit] NOT NULL,
 CONSTRAINT [PK_DfExemptItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DfMaster]    Script Date: 10/21/2016 2:22:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfMaster](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL CONSTRAINT [DF_DfMaster_Name]  DEFAULT (''),
	[Purpose] [nvarchar](100) NOT NULL CONSTRAINT [DF_DfMaster_Purpose]  DEFAULT (''),
 CONSTRAINT [PK_DfMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DfMasterData]    Script Date: 10/21/2016 2:22:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfMasterData](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DfMasterID] [int] NOT NULL CONSTRAINT [DF_DfMasterData_DfMasterID]  DEFAULT ((0)),
	[Name] [nvarchar](100) NOT NULL,
	[Value] [nvarchar](max) NOT NULL CONSTRAINT [DF_DfMasterData_Value]  DEFAULT (''),
 CONSTRAINT [PK_DfMasterData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DfRequest]    Script Date: 10/21/2016 2:22:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfRequest](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DfMasterID] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[DateRequested] [datetime2](7) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_DfRequest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[DfRequestData]    Script Date: 10/21/2016 2:22:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfRequestData](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[DfRequestID] [bigint] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](300) NOT NULL,
 CONSTRAINT [PK_DfRequestData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[DfMaster] ON 

INSERT [dbo].[DfMaster] ([ID], [Name], [Purpose]) VALUES (1, N'GST', N'Apply GST Charges')
SET IDENTITY_INSERT [dbo].[DfMaster] OFF
SET IDENTITY_INSERT [dbo].[DfMasterData] ON 

INSERT [dbo].[DfMasterData] ([ID], [DfMasterID], [Name], [Value]) VALUES (1, 1, N'GstPct', N'6')
INSERT [dbo].[DfMasterData] ([ID], [DfMasterID], [Name], [Value]) VALUES (2, 1, N'Rounding', N'2')
SET IDENTITY_INSERT [dbo].[DfMasterData] OFF
ALTER TABLE [dbo].[DfExemptItem] ADD  CONSTRAINT [DF_DfExemptItem_Code]  DEFAULT ('') FOR [Code]
GO
ALTER TABLE [dbo].[DfExemptItem] ADD  CONSTRAINT [DF_DfExemptItem_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[DfExemptItem] ADD  CONSTRAINT [DF_DfExemptItem_Exempt]  DEFAULT ((0)) FOR [Exempt]
GO
ALTER TABLE [dbo].[DfRequest] ADD  CONSTRAINT [DF_DfRequest_DfMasterID]  DEFAULT ((0)) FOR [DfMasterID]
GO
ALTER TABLE [dbo].[DfRequest] ADD  CONSTRAINT [DF_DfRequest_Title]  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[DfRequest] ADD  CONSTRAINT [DF_DfRequest_DateRequested]  DEFAULT (getdate()) FOR [DateRequested]
GO
ALTER TABLE [dbo].[DfRequest] ADD  CONSTRAINT [DF_DfRequest_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[DfRequest] ADD  CONSTRAINT [DF_DfRequest_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[DfRequestData] ADD  CONSTRAINT [DF_DfRequestData_DfRequestID]  DEFAULT ((0)) FOR [DfRequestID]
GO
ALTER TABLE [dbo].[DfRequestData] ADD  CONSTRAINT [DF_DfRequestData_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[DfRequestData] ADD  CONSTRAINT [DF_DfRequestData_Value]  DEFAULT ('') FOR [Value]
GO
