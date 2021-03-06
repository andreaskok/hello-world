GO
/****** Object:  Table [dbo].[Budgets]    Script Date: 10/4/2016 6:14:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Budgets](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](max) NULL,
	[Name] [nvarchar](max) NULL,
 CONSTRAINT [PK_dbo.Budgets] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[Budgets] ON 

INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (1, N'MA', N'Marketing')
INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (2, N'RD', N'Research and Development')
INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (3, N'PJ', N'Project Support and Enhancement')
INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (4, N'FI', N'Finance')
INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (5, N'CT', N'Construction')
INSERT [dbo].[Budgets] ([ID], [Code], [Name]) VALUES (6, N'AD', N'Administration and Legal')
SET IDENTITY_INSERT [dbo].[Budgets] OFF
