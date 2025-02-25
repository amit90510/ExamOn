CREATE TABLE [dbo].[tblactivityHistory](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Activity] [varchar](1000) NOT NULL,
	[Module] [varchar](100) NOT NULL,
	[createdate] [date] NOT NULL,
	[UserId] [bigint] NOT NULL,
 CONSTRAINT [PK_tblactivityHistory] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblAnswers]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblAnswers](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Question] [bigint] NOT NULL,
	[OptionText] [varchar](1000) NOT NULL,
 CONSTRAINT [PK_tblAnswers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblBatch]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblBatch](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Batch] [varchar](50) NOT NULL,
	[Class] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblBatch] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblClass]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblClass](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[ClassName] [varchar](50) NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblshift] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblEmailCredientials]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEmailCredientials](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[EmailFromAddress] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblEmailCredientials] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblexam]    Script Date: 24-12-2023 12:19:06 PM ******/
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
 CONSTRAINT [PK_tblexam] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblexamLive]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblexamLive](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Exam] [bigint] NOT NULL,
	[Question] [bigint] NOT NULL,
	[ChoosedOption] [int] NULL,
	[TimeTaken] [varchar](50) NULL,
 CONSTRAINT [PK_tblexamLive] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblexamQuestions]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblexamQuestions](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Sections] [bigint] NOT NULL,
	[Question] [bigint] NOT NULL,
	[PlusMarks] [int] NOT NULL,
	[NegativeMark] [int] NOT NULL,
 CONSTRAINT [PK_tblexamQuestions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblexamSections]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblexamSections](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Exam] [bigint] NOT NULL,
	[Sections] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblexamSections] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblexamStudents]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblexamStudents](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Exam] [bigint] NOT NULL,
	[UserName] [bigint] NOT NULL,
 CONSTRAINT [PK_tblexamStudents] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbllogin]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbllogin](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[UserName] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[EmailId] [varchar](100) NOT NULL,
	[Mobile] [varchar](50) NULL,
	[Active] [bit] NOT NULL,
	[BlockLogin] [bit] NULL,
	[CreatedOn] [datetime] NULL,
	[TenantToken] [varchar](100) NOT NULL,
	[BlockMessage] [varbinary](500) NULL,
	[LoginType] [int] NOT NULL,
 CONSTRAINT [PK_tbllogin] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblloginType]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblloginType](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [varchar](10) NOT NULL,
	[TypeName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblloginType] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblQuestions]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblQuestions](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Question] [varbinary](max) NOT NULL,
	[QuestionType] [varchar](5) NOT NULL,
	[CorrectAnswer] [bigint] NOT NULL,
 CONSTRAINT [PK_tblQuestions] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblSection]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSection](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Section] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblSection] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblshift]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblshift](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[Shift] [varchar](50) NOT NULL,
	[Batch] [bigint] NOT NULL,
	[Active] [bit] NOT NULL,
 CONSTRAINT [PK_tblshift_1] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbltenant]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbltenant](
	[id] [varchar](100) NOT NULL,
	[TenantName] [varchar](100) NOT NULL,
	[TenantAddress] [varchar](500) NULL,
	[TenantEmail] [varchar](100) NULL,
	[TenantMobile] [varchar](20) NULL,
	[CreatedOn] [date] NOT NULL,
	[SubscriptionEndDate] [date] NOT NULL,
	[SubscriptionEndMessage] [varchar](500) NULL,
	[LastRechargeOn] [date] NOT NULL,
	[RechargeAmount] [bigint] NOT NULL,
 CONSTRAINT [PK_tbltenant] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbluserProfile]    Script Date: 24-12-2023 12:19:06 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbluserProfile](
	[id] [bigint] NOT NULL,
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
SET IDENTITY_INSERT [dbo].[tblloginType] ON 

INSERT [dbo].[tblloginType] ([id], [Type], [TypeName]) VALUES (1, N'S', N'Student')
INSERT [dbo].[tblloginType] ([id], [Type], [TypeName]) VALUES (2, N'A', N'Admin')
INSERT [dbo].[tblloginType] ([id], [Type], [TypeName]) VALUES (3, N'W', N'WebMaster')
INSERT [dbo].[tblloginType] ([id], [Type], [TypeName]) VALUES (4, N'T', N'Teacher')
SET IDENTITY_INSERT [dbo].[tblloginType] OFF
GO
/****** Object:  Index [idx_tblbatch_class]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tblbatch_class] ON [dbo].[tblBatch]
(
	[Class] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tblEmailCredientials]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tblEmailCredientials] ON [dbo].[tblEmailCredientials]
(
	[EmailFromAddress] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [idx_tblexamlive]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tblexamlive] ON [dbo].[tblexamLive]
(
	[Exam] ASC
)
INCLUDE([Question]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [idx_tblexamQuestions]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tblexamQuestions] ON [dbo].[tblexamQuestions]
(
	[Sections] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [idx_tblexamSections]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tblexamSections] ON [dbo].[tblexamSections]
(
	[Exam] ASC
)
INCLUDE([Sections]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [idx_tblexamStudents]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tblexamStudents] ON [dbo].[tblexamStudents]
(
	[Exam] ASC
)
INCLUDE([UserName]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_tbllogin_userPass]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tbllogin_userPass] ON [dbo].[tbllogin]
(
	[UserName] ASC
)
INCLUDE([Password]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_tbllogin]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [IX_tbllogin] ON [dbo].[tbllogin]
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [idx_tbluserprofile_user]    Script Date: 24-12-2023 12:19:06 PM ******/
CREATE NONCLUSTERED INDEX [idx_tbluserprofile_user] ON [dbo].[tbluserProfile]
(
	[UserName] ASC
)
INCLUDE([Shift]) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblactivityHistory] ADD  CONSTRAINT [DF_tblactivityHistory_createdate]  DEFAULT (getdate()) FOR [createdate]
GO
ALTER TABLE [dbo].[tblBatch] ADD  CONSTRAINT [DF_tblBatch_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[tblClass] ADD  CONSTRAINT [DF_tblClass_active]  DEFAULT ((1)) FOR [Active]
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
ALTER TABLE [dbo].[tblexamQuestions] ADD  CONSTRAINT [DF_tblexamQuestions_PlusMarks]  DEFAULT ((1)) FOR [PlusMarks]
GO
ALTER TABLE [dbo].[tblexamQuestions] ADD  CONSTRAINT [DF_tblexamQuestions_NegativeMark]  DEFAULT ((1)) FOR [NegativeMark]
GO
ALTER TABLE [dbo].[tblexamSections] ADD  CONSTRAINT [DF_tblexamSections_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[tbllogin] ADD  CONSTRAINT [DF_tbllogin_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[tbllogin] ADD  CONSTRAINT [DF_tbllogin_BlockLogin]  DEFAULT ((0)) FOR [BlockLogin]
GO
ALTER TABLE [dbo].[tbllogin] ADD  CONSTRAINT [DF_tbllogin_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[tblQuestions] ADD  CONSTRAINT [DF_tblQuestions_QuestionType]  DEFAULT ('Text') FOR [QuestionType]
GO
ALTER TABLE [dbo].[tblshift] ADD  CONSTRAINT [DF_tblshift_Active]  DEFAULT ((1)) FOR [Active]
GO
ALTER TABLE [dbo].[tbltenant] ADD  CONSTRAINT [DF_tbltenant_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[tbltenant] ADD  CONSTRAINT [DF_tbltenant_SubscriptionEndDate]  DEFAULT (getdate()+(15)) FOR [SubscriptionEndDate]
GO
ALTER TABLE [dbo].[tbltenant] ADD  CONSTRAINT [DF_tbltenant_SubscriptionEndMessage]  DEFAULT ('your subscription to this access has been ended, Please contact your administrator') FOR [SubscriptionEndMessage]
GO
ALTER TABLE [dbo].[tbltenant] ADD  CONSTRAINT [DF_tbltenant_LastRechargeOn]  DEFAULT (getdate()) FOR [LastRechargeOn]
GO
ALTER TABLE [dbo].[tbltenant] ADD  CONSTRAINT [DF_tbltenant_rechargeAmount]  DEFAULT ((0)) FOR [RechargeAmount]
GO
ALTER TABLE [dbo].[tblactivityHistory]  WITH CHECK ADD  CONSTRAINT [FK_tblactivityHistory_tbllogin] FOREIGN KEY([UserId])
REFERENCES [dbo].[tbllogin] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblactivityHistory] CHECK CONSTRAINT [FK_tblactivityHistory_tbllogin]
GO
ALTER TABLE [dbo].[tblAnswers]  WITH CHECK ADD  CONSTRAINT [FK_tblAnswers_tblQuestions] FOREIGN KEY([Question])
REFERENCES [dbo].[tblQuestions] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblAnswers] CHECK CONSTRAINT [FK_tblAnswers_tblQuestions]
GO
ALTER TABLE [dbo].[tblBatch]  WITH CHECK ADD  CONSTRAINT [FK_tblBatch_tblClass] FOREIGN KEY([Class])
REFERENCES [dbo].[tblClass] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblBatch] CHECK CONSTRAINT [FK_tblBatch_tblClass]
GO
ALTER TABLE [dbo].[tblexamLive]  WITH CHECK ADD  CONSTRAINT [FK_tblexamLive_tblexam] FOREIGN KEY([Exam])
REFERENCES [dbo].[tblexam] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamLive] CHECK CONSTRAINT [FK_tblexamLive_tblexam]
GO
ALTER TABLE [dbo].[tblexamLive]  WITH CHECK ADD  CONSTRAINT [FK_tblexamLive_tblQuestions] FOREIGN KEY([Question])
REFERENCES [dbo].[tblQuestions] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamLive] CHECK CONSTRAINT [FK_tblexamLive_tblQuestions]
GO
ALTER TABLE [dbo].[tblexamQuestions]  WITH CHECK ADD  CONSTRAINT [FK_tblexamQuestions_tblexamSections] FOREIGN KEY([Sections])
REFERENCES [dbo].[tblexamSections] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamQuestions] CHECK CONSTRAINT [FK_tblexamQuestions_tblexamSections]
GO
ALTER TABLE [dbo].[tblexamQuestions]  WITH CHECK ADD  CONSTRAINT [FK_tblexamQuestions_tblQuestions] FOREIGN KEY([Question])
REFERENCES [dbo].[tblQuestions] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamQuestions] CHECK CONSTRAINT [FK_tblexamQuestions_tblQuestions]
GO
ALTER TABLE [dbo].[tblexamSections]  WITH CHECK ADD  CONSTRAINT [FK_tblexamSections_tblexam] FOREIGN KEY([Exam])
REFERENCES [dbo].[tblexam] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamSections] CHECK CONSTRAINT [FK_tblexamSections_tblexam]
GO
ALTER TABLE [dbo].[tblexamSections]  WITH CHECK ADD  CONSTRAINT [FK_tblexamSections_tblSection] FOREIGN KEY([Sections])
REFERENCES [dbo].[tblSection] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamSections] CHECK CONSTRAINT [FK_tblexamSections_tblSection]
GO
ALTER TABLE [dbo].[tblexamStudents]  WITH CHECK ADD  CONSTRAINT [FK_tblexamStudents_tblexam] FOREIGN KEY([Exam])
REFERENCES [dbo].[tblexam] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamStudents] CHECK CONSTRAINT [FK_tblexamStudents_tblexam]
GO
ALTER TABLE [dbo].[tblexamStudents]  WITH CHECK ADD  CONSTRAINT [FK_tblexamStudents_tbllogin] FOREIGN KEY([UserName])
REFERENCES [dbo].[tbllogin] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblexamStudents] CHECK CONSTRAINT [FK_tblexamStudents_tbllogin]
GO
ALTER TABLE [dbo].[tbllogin]  WITH CHECK ADD  CONSTRAINT [FK_tbllogin_tblloginType] FOREIGN KEY([LoginType])
REFERENCES [dbo].[tblloginType] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbllogin] CHECK CONSTRAINT [FK_tbllogin_tblloginType]
GO
ALTER TABLE [dbo].[tbllogin]  WITH CHECK ADD  CONSTRAINT [FK_tbllogin_tbltenant] FOREIGN KEY([TenantToken])
REFERENCES [dbo].[tbltenant] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tbllogin] CHECK CONSTRAINT [FK_tbllogin_tbltenant]
GO
ALTER TABLE [dbo].[tblshift]  WITH CHECK ADD  CONSTRAINT [FK_tblshift_tblBatch] FOREIGN KEY([Batch])
REFERENCES [dbo].[tblBatch] ([id])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[tblshift] CHECK CONSTRAINT [FK_tblshift_tblBatch]
GO
ALTER TABLE [dbo].[tbluserProfile]  WITH CHECK ADD  CONSTRAINT [FK_tbluserProfile_tbllogin] FOREIGN KEY([Shift])
REFERENCES [dbo].[tblshift] ([id])
ON UPDATE SET NULL
ON DELETE SET NULL
GO
ALTER TABLE [dbo].[tbluserProfile] CHECK CONSTRAINT [FK_tbluserProfile_tbllogin]
GO
delete from tblEmailCredientials
GO
Insert into tblEmailCredientials(EmailFromAddress, Password) values('info@travoware.com', 'jlw3Aw5e+ytv1fT+prkT7Q==')
GO

