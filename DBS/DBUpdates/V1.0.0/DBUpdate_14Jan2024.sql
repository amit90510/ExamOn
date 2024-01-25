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


ALTER TABLE [dbo].[tblUserTypeAccess] DROP CONSTRAINT [FK_tblUserTypeAccess_tblloginType]
GO

ALTER TABLE [dbo].[tblUserTypeAccess] DROP CONSTRAINT [DF_tblUserTypeAccess_UpdatedOn]
GO

/****** Object:  Table [dbo].[tblUserTypeAccess]    Script Date: 17-01-2024 03:54:44 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblUserTypeAccess]') AND type in (N'U'))
DROP TABLE [dbo].[tblUserTypeAccess]
GO

/****** Object:  Table [dbo].[tblUserTypeAccess]    Script Date: 17-01-2024 03:54:44 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUserTypeAccess](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[TypeId] [int] NOT NULL,
	[UserPath] [varchar](1000) NOT NULL,
	[UpdatedOn] [date] NOT NULL,
 CONSTRAINT [PK_tblUserTypeAccess] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblUserTypeAccess] ADD  CONSTRAINT [DF_tblUserTypeAccess_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO

ALTER TABLE [dbo].[tblUserTypeAccess]  WITH CHECK ADD  CONSTRAINT [FK_tblUserTypeAccess_tblloginType] FOREIGN KEY([TypeId])
REFERENCES [dbo].[tblloginType] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tblUserTypeAccess] CHECK CONSTRAINT [FK_tblUserTypeAccess_tblloginType]
GO

Create Nonclustered index idx_typeIDUserAccess On [tblUserTypeAccess](TypeId)

GO

ALTER TABLE [dbo].[tblUserProfileImage] DROP CONSTRAINT [FK_tblUserProfileImage_tbllogin]
GO

/****** Object:  Table [dbo].[tblUserProfileImage]    Script Date: 25-01-2024 02:38:53 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblUserProfileImage]') AND type in (N'U'))
DROP TABLE [dbo].[tblUserProfileImage]
GO

/****** Object:  Table [dbo].[tblUserProfileImage]    Script Date: 25-01-2024 02:38:53 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUserProfileImage](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[ProfileImage] [varbinary](max) NULL,
	[ProfileImageName] [varchar](500) NULL,
 CONSTRAINT [PK_tblUserProfileImage_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblUserProfileImage]  WITH CHECK ADD  CONSTRAINT [FK_tblUserProfileImage_tbllogin] FOREIGN KEY([UserName])
REFERENCES [dbo].[tbllogin] ([UserName])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tblUserProfileImage] CHECK CONSTRAINT [FK_tblUserProfileImage_tbllogin]
GO
