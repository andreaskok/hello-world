GO
/****** Object:  Table [dbo].[WfAction]    Script Date: 10/11/2016 5:22:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfAction](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfActionTypeID] [int] NOT NULL CONSTRAINT [DF_WfAction_WfActionTypeID]  DEFAULT ((0)),
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfAction_WfProcessID]  DEFAULT ((0)),
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfAction_Name]  DEFAULT (''),
	[Description] [nvarchar](500) NOT NULL CONSTRAINT [DF_WfAction_Description]  DEFAULT (''),
 CONSTRAINT [PK_WfAction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfActionTarget]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfActionTarget](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfActionID] [int] NOT NULL CONSTRAINT [DF_WfActionTarget_WfActionID]  DEFAULT ((0)),
	[WfTargetID] [int] NOT NULL CONSTRAINT [DF_WfActionTarget_WfTargetID]  DEFAULT ((0)),
	[WfGroupID] [int] NOT NULL CONSTRAINT [DF_WfActionTarget_WfGroupID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfActionTarget] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfActionType]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfActionType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfActionType_Name]  DEFAULT (''),
 CONSTRAINT [PK_WfActionType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfActivity]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfActivity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfActivityTypeID] [int] NOT NULL CONSTRAINT [DF_WfActivity_WfActivityTypeID]  DEFAULT ((0)),
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfActivity_WfProcessID]  DEFAULT ((0)),
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfActivity_Name]  DEFAULT (''),
	[Description] [nvarchar](500) NOT NULL CONSTRAINT [DF_WfActivity_Description]  DEFAULT (''),
 CONSTRAINT [PK_WfActivity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfActivityTarget]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfActivityTarget](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfActivityID] [int] NOT NULL,
	[WfTargetID] [int] NOT NULL,
	[WfGroupID] [int] NOT NULL,
 CONSTRAINT [PK_WfActivityTarget] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfActivityType]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfActivityType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
 CONSTRAINT [PK_WfActivityType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfEscalation]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfEscalation](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SH_USERID] [int] NOT NULL CONSTRAINT [DF_WfEscalation_SH_USERID]  DEFAULT ((0)),
	[Mandate] [decimal](28, 2) NOT NULL CONSTRAINT [DF_WfEscalation_Mandate]  DEFAULT ((0)),
	[EscalationGroupID] [int] NOT NULL CONSTRAINT [DF_Table_1_Level1ExcalationID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfEscalation] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfGroup]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfGroup](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfGroup_WfProcessID]  DEFAULT ((0)),
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfGroup_Name]  DEFAULT (''),
 CONSTRAINT [PK_WfGroup] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfGroupMember]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfGroupMember](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfGroupID] [int] NOT NULL CONSTRAINT [DF_WfGroupMember_WfGroupID]  DEFAULT ((0)),
	[SH_USERID] [int] NOT NULL CONSTRAINT [DF_WfGroupMember_SH_USERID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfGroupMember] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfProcess]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfProcess](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfProcess_Name]  DEFAULT (''),
 CONSTRAINT [PK_WfProcess] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfProcessAdmin]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfProcessAdmin](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfProcessAdmin_WfProcessID]  DEFAULT ((0)),
	[SH_USERID] [int] NOT NULL CONSTRAINT [DF_WfProcessAdmin_SH_USERID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfProcessAdmin] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfRequest]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfRequest](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfProcessID] [int] NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[DateRequested] [datetime2](7) NOT NULL,
	[SH_USERID] [int] NOT NULL,
	[CurrentStateID] [int] NOT NULL,
	[CreateDate] [datetime2](7) NOT NULL,
	[UpdateDate] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_WfRequest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfRequestAction]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfRequestAction](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfRequestID] [bigint] NOT NULL,
	[WfActionID] [int] NOT NULL,
	[WfTransitionID] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsComplete] [bit] NOT NULL,
 CONSTRAINT [PK_WfRequestAction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfRequestData]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfRequestData](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfRequestID] [bigint] NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Value] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_WfRequestData] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfRequestFile]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[WfRequestFile](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfRequestID] [bigint] NOT NULL,
	[SH_USERID] [int] NOT NULL,
	[DateUploaded] [datetime2](7) NOT NULL,
	[FileName] [nvarchar](200) NOT NULL,
	[FileContent] [varbinary](max) NULL,
	[MIMEType] [nvarchar](100) NULL,
	[FilePath] [nvarchar](300) NULL,
 CONSTRAINT [PK_WfRequestFile] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[WfRequestNote]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfRequestNote](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfRequestID] [bigint] NOT NULL,
	[SH_USERID] [int] NOT NULL,
	[Note] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_WfRequestNote] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfRequestStakeholder]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfRequestStakeholder](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfRequestID] [bigint] NOT NULL,
	[SH_USERID] [int] NOT NULL,
 CONSTRAINT [PK_WfRequestStakeholder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfState]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfState](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfStateTypeID] [int] NOT NULL CONSTRAINT [DF_WfState_WfStateTypeID]  DEFAULT ((0)),
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfState_WfProcessID]  DEFAULT ((0)),
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfState_Name]  DEFAULT (''),
	[Description] [nvarchar](500) NOT NULL CONSTRAINT [DF_WfState_Description]  DEFAULT (''),
 CONSTRAINT [PK_WfState] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfStateActivity]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfStateActivity](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[WfStateID] [int] NOT NULL,
	[WfActivityID] [int] NOT NULL,
 CONSTRAINT [PK_WfStateActivity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfStateType]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfStateType](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL CONSTRAINT [DF_WfStateType_Name]  DEFAULT (''),
 CONSTRAINT [PK_WfStateType] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfTarget]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfTarget](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL CONSTRAINT [DF_WfTarget_Name]  DEFAULT (''),
	[Description] [nvarchar](500) NOT NULL CONSTRAINT [DF_WfTarget_Description]  DEFAULT (''),
 CONSTRAINT [PK_WfTarget] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfTransition]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfTransition](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfProcessID] [int] NOT NULL CONSTRAINT [DF_WfTransition_WfProcessID]  DEFAULT ((0)),
	[CurrentStateID] [int] NOT NULL CONSTRAINT [DF_WfTransition_CurrentStateID]  DEFAULT ((0)),
	[NextStateID] [int] NOT NULL CONSTRAINT [DF_WfTransition_NextStateID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfTransition] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfTransitionAction]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfTransitionAction](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfTransitionID] [bigint] NOT NULL CONSTRAINT [DF_WfTransitionAction_WfTransitionID]  DEFAULT ((0)),
	[WfActionID] [int] NOT NULL CONSTRAINT [DF_WfTransitionAction_WfActionID]  DEFAULT ((0)),
 CONSTRAINT [PK_WfTransitionAction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WfTransitionActivity]    Script Date: 10/11/2016 5:22:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WfTransitionActivity](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[WfTransitionID] [bigint] NOT NULL,
	[WfActivityID] [int] NOT NULL,
 CONSTRAINT [PK_WfTransitionActivity] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[WfAction] ON 

INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (1, 1, 1, N'Start Payment', N'')
INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (2, 2, 1, N'Submit Payment', N'')
INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (3, 3, 1, N'Deny Payment', N'')
INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (4, 1, 1, N'Recommend Payment', N'')
INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (5, 1, 1, N'Approve Payment', N'')
INSERT [dbo].[WfAction] ([ID], [WfActionTypeID], [WfProcessID], [Name], [Description]) VALUES (6, 1, 1, N'Cancel Payment', N'')
SET IDENTITY_INSERT [dbo].[WfAction] OFF
SET IDENTITY_INSERT [dbo].[WfActionTarget] ON 

INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (1, 1, 4, 1)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (2, 2, 2, 1)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (3, 2, 4, 1)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (4, 3, 1, 2)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (5, 3, 4, 2)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (6, 4, 3, 2)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (7, 4, 4, 2)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (8, 5, 1, 3)
INSERT [dbo].[WfActionTarget] ([ID], [WfActionID], [WfTargetID], [WfGroupID]) VALUES (9, 5, 4, 3)
SET IDENTITY_INSERT [dbo].[WfActionTarget] OFF
SET IDENTITY_INSERT [dbo].[WfActionType] ON 

INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (1, N'Start')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (2, N'Submit')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (3, N'Deny')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (4, N'Recommend')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (5, N'Approve')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (6, N'Cancel')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (7, N'Add Note')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (8, N'Send Email')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (9, N'Add Stakeholder')
INSERT [dbo].[WfActionType] ([ID], [Name]) VALUES (10, N'Remove Stakeholder')
SET IDENTITY_INSERT [dbo].[WfActionType] OFF
SET IDENTITY_INSERT [dbo].[WfActivity] ON 

INSERT [dbo].[WfActivity] ([ID], [WfActivityTypeID], [WfProcessID], [Name], [Description]) VALUES (1, 1, 1, N'Add note for payment', N'')
INSERT [dbo].[WfActivity] ([ID], [WfActivityTypeID], [WfProcessID], [Name], [Description]) VALUES (2, 2, 1, N'Send email for payment', N'')
INSERT [dbo].[WfActivity] ([ID], [WfActivityTypeID], [WfProcessID], [Name], [Description]) VALUES (3, 3, 1, N'Add stakeholder for payment', N'')
INSERT [dbo].[WfActivity] ([ID], [WfActivityTypeID], [WfProcessID], [Name], [Description]) VALUES (4, 4, 1, N'Remaove stakeholder for payment', N'')
SET IDENTITY_INSERT [dbo].[WfActivity] OFF
SET IDENTITY_INSERT [dbo].[WfEscalation] ON 

INSERT [dbo].[WfEscalation] ([ID], [SH_USERID], [Mandate], [EscalationGroupID]) VALUES (1, 10, CAST(100.00 AS Decimal(28, 2)), 2)
INSERT [dbo].[WfEscalation] ([ID], [SH_USERID], [Mandate], [EscalationGroupID]) VALUES (2, 9, CAST(1000.00 AS Decimal(28, 2)), 3)
INSERT [dbo].[WfEscalation] ([ID], [SH_USERID], [Mandate], [EscalationGroupID]) VALUES (3, 7, CAST(10000.00 AS Decimal(28, 2)), 3)
SET IDENTITY_INSERT [dbo].[WfEscalation] OFF
SET IDENTITY_INSERT [dbo].[WfGroup] ON 

INSERT [dbo].[WfGroup] ([ID], [WfProcessID], [Name]) VALUES (1, 1, N'Payment Request')
INSERT [dbo].[WfGroup] ([ID], [WfProcessID], [Name]) VALUES (2, 1, N'Payment Recommendation')
INSERT [dbo].[WfGroup] ([ID], [WfProcessID], [Name]) VALUES (3, 1, N'Payment Approval')
SET IDENTITY_INSERT [dbo].[WfGroup] OFF
SET IDENTITY_INSERT [dbo].[WfGroupMember] ON 

INSERT [dbo].[WfGroupMember] ([ID], [WfGroupID], [SH_USERID]) VALUES (1, 1, 10)
INSERT [dbo].[WfGroupMember] ([ID], [WfGroupID], [SH_USERID]) VALUES (2, 2, 9)
INSERT [dbo].[WfGroupMember] ([ID], [WfGroupID], [SH_USERID]) VALUES (3, 3, 7)
SET IDENTITY_INSERT [dbo].[WfGroupMember] OFF
SET IDENTITY_INSERT [dbo].[WfProcess] ON 

INSERT [dbo].[WfProcess] ([ID], [Name]) VALUES (1, N'Payment')
SET IDENTITY_INSERT [dbo].[WfProcess] OFF
SET IDENTITY_INSERT [dbo].[WfProcessAdmin] ON 

INSERT [dbo].[WfProcessAdmin] ([ID], [WfProcessID], [SH_USERID]) VALUES (1, 1, 7)
INSERT [dbo].[WfProcessAdmin] ([ID], [WfProcessID], [SH_USERID]) VALUES (2, 1, 9)
INSERT [dbo].[WfProcessAdmin] ([ID], [WfProcessID], [SH_USERID]) VALUES (3, 1, 10)
SET IDENTITY_INSERT [dbo].[WfProcessAdmin] OFF
SET IDENTITY_INSERT [dbo].[WfState] ON 

INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (1, 1, 1, N'Start Payment', N'')
INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (2, 2, 1, N'Submit Payment', N'')
INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (3, 3, 1, N'Deny Payment', N'')
INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (4, 4, 1, N'Recommend Payment', N'')
INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (5, 5, 1, N'Approve Payment', N'')
INSERT [dbo].[WfState] ([ID], [WfStateTypeID], [WfProcessID], [Name], [Description]) VALUES (6, 6, 1, N'Cancel Payment', N'')
SET IDENTITY_INSERT [dbo].[WfState] OFF
SET IDENTITY_INSERT [dbo].[WfStateType] ON 

INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (1, N'Start')
INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (2, N'Submit')
INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (3, N'Deny')
INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (4, N'Recommend')
INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (5, N'Approve')
INSERT [dbo].[WfStateType] ([ID], [Name]) VALUES (6, N'Cancel')
SET IDENTITY_INSERT [dbo].[WfStateType] OFF
SET IDENTITY_INSERT [dbo].[WfTarget] ON 

INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (1, N'Payment Requester', N'')
INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (2, N'Payment Recommender', N'')
INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (3, N'Payment Approver', N'')
INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (4, N'Payment Stakeholder', N'')
INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (5, N'Payment Group Member', N'')
INSERT [dbo].[WfTarget] ([ID], [Name], [Description]) VALUES (6, N'Payment Process Admin', N'')
SET IDENTITY_INSERT [dbo].[WfTarget] OFF
SET IDENTITY_INSERT [dbo].[WfTransition] ON 

INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (1, 1, 1, 2)
INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (2, 1, 2, 3)
INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (3, 1, 2, 4)
INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (4, 1, 4, 3)
INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (5, 1, 4, 5)
INSERT [dbo].[WfTransition] ([ID], [WfProcessID], [CurrentStateID], [NextStateID]) VALUES (6, 1, 1, 6)
SET IDENTITY_INSERT [dbo].[WfTransition] OFF
SET IDENTITY_INSERT [dbo].[WfTransitionAction] ON 

INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (1, 1, 2)
INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (2, 2, 3)
INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (3, 3, 4)
INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (4, 4, 3)
INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (5, 5, 5)
INSERT [dbo].[WfTransitionAction] ([ID], [WfTransitionID], [WfActionID]) VALUES (6, 6, 6)
SET IDENTITY_INSERT [dbo].[WfTransitionAction] OFF
ALTER TABLE [dbo].[WfActivityTarget] ADD  CONSTRAINT [DF_WfActivityTarget_WfTargetID]  DEFAULT ((0)) FOR [WfTargetID]
GO
ALTER TABLE [dbo].[WfActivityTarget] ADD  CONSTRAINT [DF_WfActivityTarget_WfGroupID]  DEFAULT ((0)) FOR [WfGroupID]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_Title]  DEFAULT ('') FOR [Title]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_DateRequested]  DEFAULT (getdate()) FOR [DateRequested]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_SH_USERID]  DEFAULT ((0)) FOR [SH_USERID]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_CurrentStateID]  DEFAULT ((0)) FOR [CurrentStateID]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
ALTER TABLE [dbo].[WfRequest] ADD  CONSTRAINT [DF_WfRequest_UpdateDate]  DEFAULT (getdate()) FOR [UpdateDate]
GO
ALTER TABLE [dbo].[WfRequestAction] ADD  CONSTRAINT [DF_WfRequestAction_WfRequestID]  DEFAULT ((0)) FOR [WfRequestID]
GO
ALTER TABLE [dbo].[WfRequestAction] ADD  CONSTRAINT [DF_WfRequestAction_WfActionID]  DEFAULT ((0)) FOR [WfActionID]
GO
ALTER TABLE [dbo].[WfRequestAction] ADD  CONSTRAINT [DF_WfRequestAction_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO
ALTER TABLE [dbo].[WfRequestAction] ADD  CONSTRAINT [DF_WfRequestAction_IsComplete]  DEFAULT ((0)) FOR [IsComplete]
GO
ALTER TABLE [dbo].[WfRequestData] ADD  CONSTRAINT [DF_WfRequestData_WfRequestID]  DEFAULT ((0)) FOR [WfRequestID]
GO
ALTER TABLE [dbo].[WfRequestData] ADD  CONSTRAINT [DF_WfRequestData_Name]  DEFAULT ('') FOR [Name]
GO
ALTER TABLE [dbo].[WfRequestData] ADD  CONSTRAINT [DF_WfRequestData_Value]  DEFAULT ('') FOR [Value]
GO
ALTER TABLE [dbo].[WfRequestFile] ADD  CONSTRAINT [DF_WfRequestFile_DateUploaded]  DEFAULT (getdate()) FOR [DateUploaded]
GO
ALTER TABLE [dbo].[WfRequestFile] ADD  CONSTRAINT [DF_WfRequestFile_FileName]  DEFAULT ('') FOR [FileName]
GO
ALTER TABLE [dbo].[WfRequestFile] ADD  CONSTRAINT [DF_WfRequestFile_FilePath]  DEFAULT ('') FOR [FilePath]
GO
ALTER TABLE [dbo].[WfRequestNote] ADD  CONSTRAINT [DF_WfRequestNote_WfRequestID]  DEFAULT ((0)) FOR [WfRequestID]
GO
ALTER TABLE [dbo].[WfRequestNote] ADD  CONSTRAINT [DF_WfRequestNote_SH_USERID]  DEFAULT ((0)) FOR [SH_USERID]
GO
ALTER TABLE [dbo].[WfRequestNote] ADD  CONSTRAINT [DF_WfRequestNote_Note]  DEFAULT ('') FOR [Note]
GO
ALTER TABLE [dbo].[WfRequestStakeholder] ADD  CONSTRAINT [DF_WfRequestStakeholder_WfRequestID]  DEFAULT ((0)) FOR [WfRequestID]
GO
ALTER TABLE [dbo].[WfRequestStakeholder] ADD  CONSTRAINT [DF_WfRequestStakeholder_SH_USERID]  DEFAULT ((0)) FOR [SH_USERID]
GO
ALTER TABLE [dbo].[WfStateActivity] ADD  CONSTRAINT [DF_WfStateActivity_WfStateID]  DEFAULT ((0)) FOR [WfStateID]
GO
ALTER TABLE [dbo].[WfStateActivity] ADD  CONSTRAINT [DF_WfStateActivity_WfActivityID]  DEFAULT ((0)) FOR [WfActivityID]
GO
ALTER TABLE [dbo].[WfTransitionActivity] ADD  CONSTRAINT [DF_WfTransitionActivity_WfTransitionID]  DEFAULT ((0)) FOR [WfTransitionID]
GO
ALTER TABLE [dbo].[WfTransitionActivity] ADD  CONSTRAINT [DF_WfTransitionActivity_WfActivityID]  DEFAULT ((0)) FOR [WfActivityID]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'WfRequest', @level2type=N'COLUMN',@level2name=N'WfProcessID'
GO
