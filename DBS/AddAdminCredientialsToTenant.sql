declare @DBToken varchar(15)
set @DBToken = '{Your Token Here}'
Insert into tbllogin(UserName, Password, TenantToken, LoginType, EmailId)
values(CONCAT(@DBToken,'-Admin'), 'YHJoI4LPGDKH11I2D5ypgA==',@DBToken, 2, 'amit90510@gmail.com')
Insert Into tbluserProfile(UserName,RealName)
Values(CONCAT(@DBToken,'-Admin'), 'WebAdmin')