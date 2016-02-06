/****** Object:  Schema [pdw]    Script Date: 06/02/2016 09:31:01 ******/
CREATE SCHEMA [pdw]
GO
/****** Object:  Schema [QTables]    Script Date: 06/02/2016 09:31:01 ******/
CREATE SCHEMA [QTables]
GO
/****** Object:  Table [dbo].[Game]    Script Date: 06/02/2016 09:31:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Game](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserIdCreator] [nvarchar](50) NULL,
	[UserIdOpponent] [nvarchar](50) NULL,
	[UserIdCurrent] [nvarchar](50) NULL,
	[UserIdWinner] [nvarchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[Board] [nvarchar](max) NULL,
	[IsTerminated] [bit] NOT NULL,
 CONSTRAINT [PK_Game] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)

GO
ALTER TABLE [dbo].[Game] ADD  CONSTRAINT [DF_Game_CreatedDate]  DEFAULT (getutcdate()) FOR [CreatedDate]
GO
ALTER TABLE [dbo].[Game] ADD  CONSTRAINT [DF_Game_IsTerminated]  DEFAULT ((0)) FOR [IsTerminated]
GO
/****** Object:  StoredProcedure [pdw].[instpdw]    Script Date: 06/02/2016 09:31:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [pdw].[instpdw]
    @DatabaseName NVARCHAR(MAX)
AS
BEGIN
    DECLARE @Sql nvarchar(max);
    SET @Sql = 'USE ' + QUOTENAME(@DatabaseName) + ';    
    PRINT ''Create schema pdw...''
    IF (SCHEMA_ID(''pdw'') IS NULL)
    BEGIN
      DECLARE @sql nvarchar(128)
      SET @sql = ''CREATE SCHEMA pdw''
      EXEC sp_executesql @sql
    END'
	EXEC sp_executesql @Sql;

    SET @Sql = 'USE ' + QUOTENAME(@DatabaseName) + ';
    PRINT ''Create schema QTables...''
    IF (SCHEMA_ID(''QTables'') IS NULL)
    BEGIN
      DECLARE @sql nvarchar(128)
      SET @sql = ''CREATE SCHEMA QTables''
      EXEC sp_executesql @sql
    END'
	EXEC sp_executesql @Sql

    EXECUTE sp_executesql @Sql
END

GO
