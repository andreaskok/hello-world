GO
/****** Object:  Table [dbo].[AccountGroup]    Script Date: 10/4/2016 4:44:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AccountGroup](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[AccGrpCode] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Status] [nvarchar](50) NULL,
	[CreateDate] [datetime2](7) NULL,
	[UpdateDate] [datetime2](7) NULL,
	[UpdateID] [nvarchar](50) NULL,
	[AccClsCode] [nvarchar](50) NULL,
 CONSTRAINT [PK_AccountGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[AccountGroup] ON 

INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (1, N'CA        ', N'CURRENT ASSETS                                                                                                                  ', N'Active', CAST(N'2016-09-02 10:54:12.1795807' AS DateTime2), CAST(N'2016-09-02 10:54:12.1795807' AS DateTime2), N'user1', NULL)
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (2, N'CL        ', N'CURRENT LIABILITIES                                                                                             ', N'Active', CAST(N'2016-06-10 11:28:30.0430000' AS DateTime2), CAST(N'2016-06-10 11:28:30.0430000' AS DateTime2), N'Carmen', NULL)
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (3, N'FA        ', N'FIXED ASSETS                                                                   ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (4, N'GC        ', N'GENERAL CHARGES                                                                                                                 ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (5, N'HQ        ', N'HQ EXPENSES                                                                                                                     ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (6, N'IV        ', N'NON CURRENT ASSETS                                                                                                              ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (7, N'LL        ', N'LONG TERM LIABILITIES                                                                                                           ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (8, N'OI        ', N'OTHER INCOMES                                                                                                                   ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (9, N'PF        ', N'PALM FACTORY                                                                                                                    ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-04-15 11:01:01.0000000' AS DateTime2), N'jenny               ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (10, N'RE        ', N'RETAINED PROFIT / LOSSES                                                                                                        ', N'Active', CAST(N'2016-06-14 18:18:24.2670000' AS DateTime2), CAST(N'2016-06-14 18:18:24.2670000' AS DateTime2), N'Carmen', NULL)
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (11, N'SC        ', N'SHARE CAPITAL                                                                                                                   ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (12, N'SI        ', N'SALES INCOMES                                                                                                                   ', N'Active', CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), CAST(N'2014-01-08 11:54:30.0000000' AS DateTime2), N'sysadm              ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (13, N'ST        ', N'STOCK                                                                                                                           ', N'Active', CAST(N'2014-07-31 08:51:37.0000000' AS DateTime2), CAST(N'2014-07-31 08:51:37.0000000' AS DateTime2), N'itech               ', N'00        ')
INSERT [dbo].[AccountGroup] ([ID], [AccGrpCode], [Description], [Status], [CreateDate], [UpdateDate], [UpdateID], [AccClsCode]) VALUES (17, N'TEST', N'FOR TESTING PURPOSE ', N'Active', CAST(N'2016-07-26 12:17:58.2196227' AS DateTime2), CAST(N'2016-07-26 12:17:58.2196227' AS DateTime2), N'Carmen', NULL)
SET IDENTITY_INSERT [dbo].[AccountGroup] OFF
