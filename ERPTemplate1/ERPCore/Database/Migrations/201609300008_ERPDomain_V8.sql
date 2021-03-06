GO
/****** Object:  Table [dbo].[Calendar]    Script Date: 10/4/2016 6:15:44 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Calendar](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[CountryStateID] [bigint] NOT NULL CONSTRAINT [DF_Calendar_CountryStateID]  DEFAULT ((0)),
	[CalendarDate] [datetime2](7) NOT NULL,
	[CalendarYear] [bigint] NOT NULL,
	[CalendarMonth] [bigint] NOT NULL,
	[CalendarDay] [bigint] NOT NULL,
	[DayOfWeekName] [nvarchar](50) NOT NULL,
	[FirstDateOfWeek] [datetime2](7) NOT NULL,
	[LastDateOfWeek] [datetime2](7) NOT NULL,
	[FirstDateOfMonth] [datetime2](7) NOT NULL,
	[LastDateOfMonth] [datetime2](7) NOT NULL,
	[FirstDateOfQuarter] [datetime2](7) NOT NULL,
	[LastDateOfQuarter] [datetime2](7) NOT NULL,
	[FirstDateOfYear] [datetime2](7) NOT NULL,
	[LastDateOfYear] [datetime2](7) NOT NULL,
	[IsBusinessDay] [bit] NOT NULL,
	[Weekend] [bit] NOT NULL,
	[Holiday] [bit] NOT NULL,
	[Weekday] [bit] NOT NULL,
	[CalendarDateDescription] [nvarchar](max) NULL,
 CONSTRAINT [PK_Calendar_1] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

