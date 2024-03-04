declare @DBToken varchar(15)
declare @DBTenantName varchar(15)
set @DBToken = '{code}'
set @DBTenantName = '{full tenant Name}'
INSERT [dbo].[tbltenant] ([id], [TenantName], [TenantAddress], [TenantEmail], [TenantMobile], [CreatedOn], [SubscriptionEndDate], [SubscriptionEndMessage], [LastRechargeOn], [RechargeAmount]) VALUES (@DBToken, @DBTenantName, N'Delhi updated', N'tenant@gmail.com', N'9874563210', CAST(N'2024-01-08' AS Date), CAST(N'2024-06-06' AS Date), N'your subscription to this access has been ended, Please contact your administrator', CAST(N'2024-02-29' AS Date), 10000)
Insert into tbllogin(UserName, Password, TenantToken, LoginType, EmailId)
values(CONCAT(@DBToken,'-WebMaster'), 'YHJoI4LPGDKH11I2D5ypgA==',@DBToken, 3, 'amit90510@gmail.com')
Insert Into tbluserProfile(UserName,RealName)
Values(CONCAT(@DBToken,'-WebMaster'), 'WebAdmin')