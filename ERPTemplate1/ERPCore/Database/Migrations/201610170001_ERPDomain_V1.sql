Go
ALTER TABLE dbo.Journal ADD IsPosted Bit NULL ;  
Go 
ALTER TABLE dbo.JournalLine ADD DebitCreditIndicator NVARCHAR(50) NULL ;  
Go 

GO
/****** Object:  Table [dbo].[MonthEndTransaction]    Script Date: 10/17/2016 12:17:31 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MonthEndTransaction](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[OrganizationID] [bigint] NOT NULL CONSTRAINT [DF_MonthEndTransaction_OrganizationID]  DEFAULT ((0)),
	[VoucherID] [bigint] NOT NULL CONSTRAINT [DF_MonthEndTransaction_VoucherID]  DEFAULT ((0)),
	[VoucherLineID] [bigint] NOT NULL CONSTRAINT [DF_MonthEndTransaction_VoucherLineID]  DEFAULT ((0)),
	[VoucherCode] [nvarchar](50) NULL,
	[VoucherLineCode] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[AccYear] [nvarchar](50) NULL,
	[AccMonth] [nvarchar](50) NULL,
	[AccCode] [nvarchar](50) NULL,
	[TransactType] [nvarchar](50) NULL,
	[DocDate] [datetime2](7) NULL,
	[DocAmt] [decimal](18, 6) NULL,
	[Quantity] [decimal](18, 0) NULL,
	[UnitPrice] [decimal](18, 6) NULL,
	[Total] [decimal](18, 6) NULL,
	[DebitCreditIndicator] [nvarchar](50) NULL,
	[PostingTable] [nvarchar](50) NULL,
	[PostingDate] [datetime2](7) NULL,
 CONSTRAINT [PK_MonthEndTransaction] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
