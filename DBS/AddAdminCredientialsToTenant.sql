declare @DBToken varchar(15)
set @DBToken = '{Your Token Here}'
Insert into tbllogin(UserName, Password, TenantToken, LoginType, EmailId)
values(CONCAT(@DBToken,'-WebMaster'), 'YHJoI4LPGDKH11I2D5ypgA==',@DBToken, 3, 'amit90510@gmail.com')
Insert Into tbluserProfile(UserName,RealName)
Values(CONCAT(@DBToken,'-WebMaster'), 'WebAdmin')