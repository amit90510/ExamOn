CREATE TABLE [dbo].[tblForgotPasswordMailCounter](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](100) NOT NULL,
	[CreatedDate] [date] NULL,
 CONSTRAINT [PK_tblForgotPasswordMailCounter] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblForgotPasswordMailCounter] ADD  CONSTRAINT [DF_tblForgotPasswordMailCounter_CreatedDate]  DEFAULT (getdate()) FOR [CreatedDate]
GO
create nonclustered index idx_forgotMailCounter on tblForgotPasswordMailCounter ([UserName]) include ([CreatedDate])
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbluserProfile]') AND type in (N'U'))
DROP TABLE [dbo].[tbluserProfile]

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tbluserProfile](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[address] [varchar](50) NULL,
	[City] [varchar](50) NULL,
	[State] [varchar](50) NULL,
	[Shift] [bigint] NULL,
	[RealName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tbluserProfile] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tbluserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tbluserProfile_tbllogin] FOREIGN KEY([Shift])
REFERENCES [dbo].[tblshift] ([id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO

ALTER TABLE [dbo].[tbluserProfile] CHECK CONSTRAINT [FK_tbluserProfile_tbllogin]
GO