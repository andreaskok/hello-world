GO
/****** Object:  Table [dbo].[ActivityGroup]    Script Date: 10/4/2016 5:22:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActivityGroup](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ActGrpCode] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Status] [nvarchar](20) NULL,
	[CreateDate] [datetime] NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateID] [nvarchar](50) NULL,
 CONSTRAINT [PK_ActivityGroup] PRIMARY KEY CLUSTERED 
(
	[ActGrpCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[ActivityGroup] ON 

INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (1, N'CA      ', N'CURRENT ASSETS                                                                                                                  ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (2, N'CL      ', N'CURRENT LIABILITIES                                                                                                             ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (3, N'FA      ', N'FIXED ASSETS                                                                                                                    ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (4, N'GC      ', N'GENERAL CHARGES                                                                                                                 ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (5, N'HQ      ', N'HQ EXPENSES                                                                                                       ', N'Active', CAST(N'2016-06-08 10:24:24.677' AS DateTime), CAST(N'2016-06-08 10:24:24.677' AS DateTime), N'Carmen')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (6, N'IV      ', N'NON CURRENT ASSETS                                                                                                              ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (7, N'LL      ', N'LONG TERM LIABILITIES                                                                                                           ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (8, N'OI      ', N'OTHER INCOMES                                                                                                                   ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (9, N'PF      ', N'PALM FACTORY                                                                                                                    ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (10, N'RE      ', N'RETAINED PROFIT / LOSSES                                                                                                        ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (11, N'SC      ', N'SHARE CAPITAL                                                                                                                   ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (12, N'SI      ', N'SALES INCOMES                                                                                                                   ', N'Active', CAST(N'2014-01-08 10:01:01.000' AS DateTime), CAST(N'2014-01-08 10:01:01.000' AS DateTime), N'sysadm              ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (13, N'ST      ', N'STOCK                                                                                                                           ', N'Active', CAST(N'2014-07-31 08:50:26.000' AS DateTime), CAST(N'2014-07-31 08:50:26.000' AS DateTime), N'itech               ')
INSERT [dbo].[ActivityGroup] ([ID], [ActGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID]) VALUES (14, N'TEST', N'TESTING PURPOSE', N'Active', CAST(N'2016-06-08 10:24:04.363' AS DateTime), CAST(N'2016-06-08 10:24:04.363' AS DateTime), N'Carmen')
SET IDENTITY_INSERT [dbo].[ActivityGroup] OFF
