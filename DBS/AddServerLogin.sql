USE [master]
GO

/* For security reasons the login is created disabled and with a random password. */
/****** Object:  Login [examon]    Script Date: 05-01-2024 02:14:14 PM ******/
CREATE LOGIN [examon] WITH PASSWORD=N'Welcome@90510', DEFAULT_DATABASE=[master], DEFAULT_LANGUAGE=[us_english], CHECK_EXPIRATION=OFF, CHECK_POLICY=ON
GO

ALTER LOGIN [examon] DISABLE
GO

ALTER SERVER ROLE [sysadmin] ADD MEMBER [examon]
GO

ALTER SERVER ROLE [setupadmin] ADD MEMBER [examon]
GO


