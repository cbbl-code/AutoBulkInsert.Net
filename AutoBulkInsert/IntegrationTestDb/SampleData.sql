CREATE TABLE [dbo].[SampleData]
(
	[Id] BIGINT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [Identifer] UNIQUEIDENTIFIER NOT NULL, 
    [Gender] CHAR(1) NOT NULL,
    [FirstName] NVARCHAR(256) NOT NULL, 
    [Surname] NVARCHAR(256) NOT NULL, 
    [DateOfBirth] DATE NOT NULL, 
    [IsMarried] BIT NOT NULL, 
    [MaidenName] NVARCHAR(50) NULL
)
