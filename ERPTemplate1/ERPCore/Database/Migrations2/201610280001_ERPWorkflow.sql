GO
/****** Object:  Table [dbo].[DfAppliedModule]    Script Date: 10/28/2016 12:21:13 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DfAppliedModule](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DfMasterID] [int] NOT NULL CONSTRAINT [DF_DfAppliedModule_DfMasterID]  DEFAULT ((0)),
	[AppliedModule] [nvarchar](100) NOT NULL CONSTRAINT [DF_DfAppliedModule_AppliedModule]  DEFAULT (''),
 CONSTRAINT [PK_DfAppliedModule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[DfAppliedModule] ON 

INSERT [dbo].[DfAppliedModule] ([ID], [DfMasterID], [AppliedModule]) VALUES (1, 1, N'Payment')
SET IDENTITY_INSERT [dbo].[DfAppliedModule] OFF