USE [ExamOn_Dummy]
GO



IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblLoginHistory]') AND type in (N'U'))
DROP TABLE [dbo].[tblLoginHistory]
ALTER TABLE [dbo].[tblLoginHistory] DROP CONSTRAINT [FK_tblLoginHistory_tbllogin]
GO

/****** Object:  Table [dbo].[tblLoginHistory]    Script Date: 15-01-2024 02:59:52 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblLoginHistory](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Ip] [varchar](50) NOT NULL,
	[Browser] [varchar](100) NOT NULL,
	[LoginDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblLoginHistory_1] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblLoginHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblLoginHistory_tbllogin] FOREIGN KEY([UserName])
REFERENCES [dbo].[tbllogin] ([UserName])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tblLoginHistory] CHECK CONSTRAINT [FK_tblLoginHistory_tbllogin]
GO

create nonclustered index idx_Noncluserted on tblloginhistory(UserName)
GO