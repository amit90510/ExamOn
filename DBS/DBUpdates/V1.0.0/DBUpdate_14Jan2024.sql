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

GO

ALTER TABLE [dbo].[tblUserShift] DROP CONSTRAINT [FK_tblUserShift_tbluserProfile]
GO

ALTER TABLE [dbo].[tblUserShift] DROP CONSTRAINT [FK_tblUserShift_tblshift]
GO

ALTER TABLE [dbo].[tblUserShift] DROP CONSTRAINT [DF_tblUserShift_Active]
GO

/****** Object:  Table [dbo].[tblUserShift]    Script Date: 27-01-2024 02:58:17 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblUserShift]') AND type in (N'U'))
DROP TABLE [dbo].[tblUserShift]
GO

/****** Object:  Table [dbo].[tblUserShift]    Script Date: 27-01-2024 02:58:17 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblUserShift](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL,
	[ShiftId] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblUserShift] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblUserShift] ADD  CONSTRAINT [DF_tblUserShift_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[tblUserShift]  WITH CHECK ADD  CONSTRAINT [FK_tblUserShift_tblshift] FOREIGN KEY([ShiftId])
REFERENCES [dbo].[tblshift] ([id])
GO

ALTER TABLE [dbo].[tblUserShift] CHECK CONSTRAINT [FK_tblUserShift_tblshift]
GO

ALTER TABLE [dbo].[tblUserShift]  WITH CHECK ADD  CONSTRAINT [FK_tblUserShift_tbluserProfile] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbluserProfile] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tblUserShift] CHECK CONSTRAINT [FK_tblUserShift_tbluserProfile]
GO

create nonclustered index idx_tblusershift_index on tbluserShift(userId) include(shiftId)

GO

ALTER TABLE [dbo].[tbluserProfile] DROP CONSTRAINT [FK_tbluserProfile_tbllogin1]
GO

ALTER TABLE [dbo].[tbluserProfile] DROP CONSTRAINT [FK_tbluserProfile_tbllogin]
GO

/****** Object:  Table [dbo].[tbluserProfile]    Script Date: 28-01-2024 01:23:46 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tbluserProfile]') AND type in (N'U'))
DROP TABLE [dbo].[tbluserProfile]
GO

/****** Object:  Table [dbo].[tbluserProfile]    Script Date: 28-01-2024 01:23:46 PM ******/
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

ALTER TABLE [dbo].[tbluserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tbluserProfile_tbllogin1] FOREIGN KEY([UserName])
REFERENCES [dbo].[tbllogin] ([UserName])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[tbluserProfile] CHECK CONSTRAINT [FK_tbluserProfile_tbllogin1]
GO
INSERT [dbo].[tblUserTypeAccess] ([TypeId], [UserPath], [UpdatedOn]) VALUES (3, N'WebAdminDashboardController/Go', CAST(N'2024-02-11' AS Date))
GO

  update tblloginType set TypeName = 'Tenant Admin' where TypeName = 'Admin'
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [FK_tblexam_tbllogin]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_CreatedOn]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_Active]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_ReviewAfterExam]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_ResultAfterExam]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_FullScreen]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_KeyBoardBlock]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_EntryAllowedTill]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_End]
GO

ALTER TABLE [dbo].[tblexam] DROP CONSTRAINT [DF_tblexam_Start]
GO

/****** Object:  Table [dbo].[tblexam]    Script Date: 12-02-2024 04:12:23 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[tblexam]') AND type in (N'U'))
DROP TABLE [dbo].[tblexam]
GO

/****** Object:  Table [dbo].[tblexam]    Script Date: 12-02-2024 04:12:23 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[tblexam](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ExamName] [varchar](500) NOT NULL,
	[Instructions] [varchar](1500) NULL,
	[StartExam] [datetime] NOT NULL,
	[EndExam] [datetime] NOT NULL,
	[EntryAllowedTill] [datetime] NOT NULL,
	[KeyBoardBlock] [bit] NOT NULL,
	[FullScreen] [bit] NOT NULL,
	[ResultAfterExam] [bit] NOT NULL,
	[ReviewAfterExam] [bit] NOT NULL,
	[Active] [bit] NOT NULL,
	[UpdatedOn] [datetime] NOT NULL,
	[SetByUserId] [bigint] NULL,
 CONSTRAINT [PK_tblexam] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_Start]  DEFAULT (getdate()) FOR [StartExam]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_End]  DEFAULT (dateadd(day,(1),getdate())) FOR [EndExam]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_EntryAllowedTill]  DEFAULT (dateadd(minute,(15),getdate())) FOR [EntryAllowedTill]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_KeyBoardBlock]  DEFAULT ((1)) FOR [KeyBoardBlock]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_FullScreen]  DEFAULT ((1)) FOR [FullScreen]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_ResultAfterExam]  DEFAULT ((1)) FOR [ResultAfterExam]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_ReviewAfterExam]  DEFAULT ((1)) FOR [ReviewAfterExam]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[tblexam] ADD  CONSTRAINT [DF_tblexam_CreatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO

ALTER TABLE [dbo].[tblexam]  WITH CHECK ADD  CONSTRAINT [FK_tblexam_tbllogin] FOREIGN KEY([SetByUserId])
REFERENCES [dbo].[tbllogin] ([id])
GO

ALTER TABLE [dbo].[tblexam] CHECK CONSTRAINT [FK_tblexam_tbllogin]
GO

CREATE TABLE [dbo].[tblTeacherInstructorShifts](
	[id] [bigint] NOT NULL,
	[LoginId] [bigint] NOT NULL,
	[ShiftId] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
	[CreatedOn] [date] NOT NULL,
	[UpdatedOn] [date] NOT NULL,
 CONSTRAINT [PK_tblTeacherInstructorShifts] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts] ADD  CONSTRAINT [DF_tblTeacherInstructorShifts_Active]  DEFAULT ((1)) FOR [Active]
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts] ADD  CONSTRAINT [DF_tblTeacherInstructorShifts_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts] ADD  CONSTRAINT [DF_tblTeacherInstructorShifts_UpdatedOn]  DEFAULT (getdate()) FOR [UpdatedOn]
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts]  WITH CHECK ADD  CONSTRAINT [FK_tblTeacherInstructorShifts_tbllogin] FOREIGN KEY([LoginId])
REFERENCES [dbo].[tbllogin] ([id])
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts] CHECK CONSTRAINT [FK_tblTeacherInstructorShifts_tbllogin]
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts]  WITH CHECK ADD  CONSTRAINT [FK_tblTeacherInstructorShifts_tblshift] FOREIGN KEY([ShiftId])
REFERENCES [dbo].[tblshift] ([id])
GO

ALTER TABLE [dbo].[tblTeacherInstructorShifts] CHECK CONSTRAINT [FK_tblTeacherInstructorShifts_tblshift]
GO