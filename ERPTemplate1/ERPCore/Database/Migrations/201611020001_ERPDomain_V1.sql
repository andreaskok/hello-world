GO
/****** Object:  Table [dbo].[JobSchedule]    Script Date: 11/2/2016 11:03:20 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[JobSchedule](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[JobCode] [nvarchar](50) NULL,
	[JobName] [nvarchar](max) NULL,
	[JobType] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[ExecuteFrequence] [nvarchar](50) NULL,
	[ExecuteDateStart] [datetime2](7) NULL,
	[ExecuteDateEnd] [datetime2](7) NULL,
	[LastRunDateTime] [datetime2](7) NULL,
	[NextRunDateTime] [datetime2](7) NULL,
	[ExecuteFlag] [int] NULL,
	[EnabledFlag] [bit] NULL,
 CONSTRAINT [PK_JobSchedule] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
