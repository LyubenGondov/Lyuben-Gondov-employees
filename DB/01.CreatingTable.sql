USE [WorkingOnProject]
GO
/****** Object:  Table [dbo].[EmployeeDaysWork]    Script Date: 21.6.2021 ã. 0:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmployeeDaysWork](
	[EmployeeId] [int] NOT NULL,
	[ProjectId] [int] NOT NULL,
	[DateFrom] [varchar](50) NULL,
	[DateTo] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[uspGetTotalWorkDays]    Script Date: 21.6.2021 ã. 0:37:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO