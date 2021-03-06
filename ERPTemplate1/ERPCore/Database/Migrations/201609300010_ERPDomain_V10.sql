GO
/****** Object:  Table [dbo].[ChartOfAccount_Dim_Setup]    Script Date: 10/4/2016 6:18:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartOfAccount_Dim_Setup](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChartOfAccountID] [bigint] NOT NULL,
	[Dimension_SettingID] [bigint] NOT NULL CONSTRAINT [DF_ChartOfAccount_Dim_Setup_Dimension_SettingID]  DEFAULT ((0)),
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChartOfAccount_Dim_Setup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChartOfAccount_Dim_Value]    Script Date: 10/4/2016 6:18:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartOfAccount_Dim_Value](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChartOfAccount_Dim_SetupID] [bigint] NOT NULL,
	[DimensionValue] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChartOfAccount_Dim_Value] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[ChartOfAccount_DimensionValue]    Script Date: 10/4/2016 6:18:22 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ChartOfAccount_DimensionValue](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ChartOfAccountID] [bigint] NOT NULL,
	[AccCode] [nvarchar](20) NOT NULL,
	[DimensionCode] [nvarchar](50) NOT NULL,
	[DimensionValue] [nvarchar](50) NOT NULL,
	[Status] [nvarchar](50) NULL,
 CONSTRAINT [PK_ChartOfAccount_DimensionValue] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[ChartOfAccount_Dim_Setup] ON 

INSERT [dbo].[ChartOfAccount_Dim_Setup] ([ID], [ChartOfAccountID], [Dimension_SettingID], [Status]) VALUES (1, 3, 2, N'Active')
INSERT [dbo].[ChartOfAccount_Dim_Setup] ([ID], [ChartOfAccountID], [Dimension_SettingID], [Status]) VALUES (2, 3, 4, N'Active')
SET IDENTITY_INSERT [dbo].[ChartOfAccount_Dim_Setup] OFF
SET IDENTITY_INSERT [dbo].[ChartOfAccount_Dim_Value] ON 

INSERT [dbo].[ChartOfAccount_Dim_Value] ([ID], [ChartOfAccount_Dim_SetupID], [DimensionValue], [Status]) VALUES (2, 1, N'ValueC', N'Active')
INSERT [dbo].[ChartOfAccount_Dim_Value] ([ID], [ChartOfAccount_Dim_SetupID], [DimensionValue], [Status]) VALUES (5, 2, N'ValueI', N'Active')
INSERT [dbo].[ChartOfAccount_Dim_Value] ([ID], [ChartOfAccount_Dim_SetupID], [DimensionValue], [Status]) VALUES (6, 2, N'ValueJ', N'Active')
INSERT [dbo].[ChartOfAccount_Dim_Value] ([ID], [ChartOfAccount_Dim_SetupID], [DimensionValue], [Status]) VALUES (7, 1, N'ValueA', NULL)
SET IDENTITY_INSERT [dbo].[ChartOfAccount_Dim_Value] OFF
SET IDENTITY_INSERT [dbo].[ChartOfAccount_DimensionValue] ON 

INSERT [dbo].[ChartOfAccount_DimensionValue] ([ID], [ChartOfAccountID], [AccCode], [DimensionCode], [DimensionValue], [Status]) VALUES (1, 3, N'CA02001', N'Block', N'N0016', N'Active')
INSERT [dbo].[ChartOfAccount_DimensionValue] ([ID], [ChartOfAccountID], [AccCode], [DimensionCode], [DimensionValue], [Status]) VALUES (2, 3, N'CA02001', N'SubBlock', N'SB006', N'Active')
SET IDENTITY_INSERT [dbo].[ChartOfAccount_DimensionValue] OFF
