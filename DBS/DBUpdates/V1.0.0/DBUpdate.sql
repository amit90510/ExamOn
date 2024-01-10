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