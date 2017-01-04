GO
/****** Object:  Table [dbo].[UserPreference]    Script Date: 10/5/2016 12:08:58 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserPreference](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[SH_USERID] [bigint] NOT NULL CONSTRAINT [DF_UserPreference_SH_USERID]  DEFAULT ((0)),
	[Language] [nvarchar](50) NOT NULL,
	[Theme] [nvarchar](50) NOT NULL,
	[RowPerPage] [nvarchar](50) NOT NULL,
	[DateFormat] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_UserPreference] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[UserPreference] ON 

GO
INSERT [dbo].[UserPreference] ([ID], [SH_USERID], [Language], [Theme], [RowPerPage], [DateFormat]) VALUES (1, 14, N'EN', N'Blue', N'20', N'DD/MM/YYYY')
GO
SET IDENTITY_INSERT [dbo].[UserPreference] OFF
GO
