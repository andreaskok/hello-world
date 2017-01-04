GO
/****** Object:  Table [dbo].[Employee]    Script Date: 10/26/2016 9:48:54 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employee](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[EmployeeCode] [nvarchar](50) NOT NULL CONSTRAINT [DF_Employee_EmployeeCode]  DEFAULT (''),
	[EmployeeName] [nvarchar](200) NOT NULL CONSTRAINT [DF_Employee_EmployeeName]  DEFAULT (''),
	[Category] [nvarchar](100) NOT NULL CONSTRAINT [DF_Employee_Category]  DEFAULT (''),
 CONSTRAINT [PK_Employee] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO