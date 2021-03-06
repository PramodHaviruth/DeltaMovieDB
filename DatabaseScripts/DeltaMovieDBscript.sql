USE [master]
GO
/****** Object:  Database [DeltaMovieDB]    Script Date: 12/1/2018 12:27:23 AM ******/
CREATE DATABASE [DeltaMovieDB]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'DeltaMovieDB', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\DeltaMovieDB.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'DeltaMovieDB_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL14.SQLEXPRESS\MSSQL\DATA\DeltaMovieDB_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [DeltaMovieDB] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [DeltaMovieDB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [DeltaMovieDB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET ARITHABORT OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [DeltaMovieDB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [DeltaMovieDB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [DeltaMovieDB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [DeltaMovieDB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [DeltaMovieDB] SET  MULTI_USER 
GO
ALTER DATABASE [DeltaMovieDB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [DeltaMovieDB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [DeltaMovieDB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [DeltaMovieDB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [DeltaMovieDB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [DeltaMovieDB] SET QUERY_STORE = OFF
GO
USE [DeltaMovieDB]
GO
ALTER DATABASE SCOPED CONFIGURATION SET IDENTITY_CACHE = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION SET LEGACY_CARDINALITY_ESTIMATION = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET LEGACY_CARDINALITY_ESTIMATION = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 0;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET MAXDOP = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET PARAMETER_SNIFFING = ON;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET PARAMETER_SNIFFING = PRIMARY;
GO
ALTER DATABASE SCOPED CONFIGURATION SET QUERY_OPTIMIZER_HOTFIXES = OFF;
GO
ALTER DATABASE SCOPED CONFIGURATION FOR SECONDARY SET QUERY_OPTIMIZER_HOTFIXES = PRIMARY;
GO
USE [DeltaMovieDB]
GO
/****** Object:  Table [dbo].[Actor]    Script Date: 12/1/2018 12:27:24 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Actor](
	[ActorId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Sex] [varchar](7) NOT NULL,
	[DOB] [date] NOT NULL,
	[Bio] [text] NOT NULL,
 CONSTRAINT [PK_Actor] PRIMARY KEY CLUSTERED 
(
	[ActorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ActorMovie]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ActorMovie](
	[ActorMovieId] [int] IDENTITY(1,1) NOT NULL,
	[ActorId] [int] NOT NULL,
	[MovieId] [int] NOT NULL,
 CONSTRAINT [PK_ActorMovie] PRIMARY KEY CLUSTERED 
(
	[ActorMovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Movie]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Movie](
	[MovieId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[YearOfRelease] [int] NOT NULL,
	[Plot] [text] NOT NULL,
	[Poster] [image] NOT NULL,
	[ProducerId] [int] NOT NULL,
 CONSTRAINT [PK_Movie] PRIMARY KEY CLUSTERED 
(
	[MovieId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Producer]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Producer](
	[ProducerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Sex] [varchar](7) NOT NULL,
	[DOB] [date] NOT NULL,
	[Bio] [text] NOT NULL,
 CONSTRAINT [PK_Producer] PRIMARY KEY CLUSTERED 
(
	[ProducerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ActorMovie]  WITH CHECK ADD  CONSTRAINT [FK_ActorMovie_Actor] FOREIGN KEY([ActorId])
REFERENCES [dbo].[Actor] ([ActorId])
GO
ALTER TABLE [dbo].[ActorMovie] CHECK CONSTRAINT [FK_ActorMovie_Actor]
GO
ALTER TABLE [dbo].[ActorMovie]  WITH CHECK ADD  CONSTRAINT [FK_ActorMovie_Movie] FOREIGN KEY([MovieId])
REFERENCES [dbo].[Movie] ([MovieId])
GO
ALTER TABLE [dbo].[ActorMovie] CHECK CONSTRAINT [FK_ActorMovie_Movie]
GO
ALTER TABLE [dbo].[Movie]  WITH CHECK ADD  CONSTRAINT [FK_Movie_Producer] FOREIGN KEY([ProducerId])
REFERENCES [dbo].[Producer] ([ProducerId])
GO
ALTER TABLE [dbo].[Movie] CHECK CONSTRAINT [FK_Movie_Producer]
GO
/****** Object:  StoredProcedure [dbo].[GetActorsinMovieList]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetActorsinMovieList]
@MovieId int
as

select A.*
from  ActorMovie AM
join Actor A on AM.ActorId = A.ActorId
where MovieId = @MovieId;
GO
/****** Object:  StoredProcedure [dbo].[GetActorsList]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[GetActorsList]

as

select * from  Actor;
GO
/****** Object:  StoredProcedure [dbo].[GetMoviesList]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetMoviesList]

as

select M.* , P.Name as ProducerName
from  Movie M
join Producer P on M.ProducerId = p.ProducerId
GO
/****** Object:  StoredProcedure [dbo].[GetProducersList]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[GetProducersList]

as

select * from  Producer;
GO
/****** Object:  StoredProcedure [dbo].[InsertActorDetails]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[InsertActorDetails]
@Name varchar(50),
@Sex varchar(7),
@DOB datetime,
@Bio text
as

Insert into Actor values(@Name, @Sex, @DOB, @Bio)

IF @@ERROR <> 0
BEGIN      
    ROLLBACK     
    RETURN 
END   
GO
/****** Object:  StoredProcedure [dbo].[InsertActorMovieDetails]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[InsertActorMovieDetails]
@ActorId int,
@MovieId int
as

Insert into ActorMovie values(@ActorId, @MovieId)

IF @@ERROR <> 0
BEGIN      
    ROLLBACK     
    RETURN 
END   
GO
/****** Object:  StoredProcedure [dbo].[InsertMovieDetails]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[InsertMovieDetails]
@Name varchar(50),
@YearOfRelease int,
@Plot text,
@Poster Image,
@ProducerId int,
@MovieId int OUTPUT
as

Insert into Movie values(@Name, @YearOfRelease, @Plot, @Poster, @ProducerId)

DECLARE @ID int

set @MovieId = SCOPE_IDENTITY()    

IF @@ERROR <> 0
BEGIN      
    ROLLBACK     
    RETURN 
END   
GO
/****** Object:  StoredProcedure [dbo].[InsertProducerDetails]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[InsertProducerDetails]
@Name varchar(50),
@Sex varchar(7),
@DOB datetime,
@Bio text
as

Insert into Producer values(@Name, @Sex, @DOB, @Bio)

IF @@ERROR <> 0
BEGIN      
    ROLLBACK     
    RETURN 
END   
GO
/****** Object:  StoredProcedure [dbo].[UpdateMovieDetails]    Script Date: 12/1/2018 12:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[UpdateMovieDetails]
@MovieId int,
@Name varchar(50),
@YearOfRelease int,
@Plot text,
@Poster Image,
@ProducerId int
as

update Movie
set Name = @Name,
	YearOfRelease = @YearOfRelease,
	Plot = @Plot,
	Poster = @Poster,
	ProducerId = @ProducerId
	where MovieId = @MovieId
GO
USE [master]
GO
ALTER DATABASE [DeltaMovieDB] SET  READ_WRITE 
GO
