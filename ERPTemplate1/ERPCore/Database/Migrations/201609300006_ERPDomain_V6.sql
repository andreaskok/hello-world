GO
/****** Object:  Table [dbo].[BillParties]    Script Date: 10/4/2016 5:27:41 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BillParties](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[BillPartyCode] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](300) NOT NULL,
	[BudgetCode] [nvarchar](50) NOT NULL CONSTRAINT [DF_BillParties_BudgetCode]  DEFAULT (''),
 CONSTRAINT [PK_BillParty] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET IDENTITY_INSERT [dbo].[BillParties] ON 

INSERT [dbo].[BillParties] ([ID], [BillPartyCode], [Name], [BudgetCode]) VALUES (1, N'Bill1', N'Bill1 Name', N'MA')
INSERT [dbo].[BillParties] ([ID], [BillPartyCode], [Name], [BudgetCode]) VALUES (2, N'Bill2', N'Bill2 Name', N'MA')
INSERT [dbo].[BillParties] ([ID], [BillPartyCode], [Name], [BudgetCode]) VALUES (3, N'Bill3', N'Bill3 Name', N'MA')
SET IDENTITY_INSERT [dbo].[BillParties] OFF
