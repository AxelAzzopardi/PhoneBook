USE [master]
GO
/****** Object:  Database [PhoneBook]    Script Date: 29/01/2023 17:28:38 ******/
CREATE DATABASE [PhoneBook]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'PhoneBook', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PhoneBook.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'PhoneBook_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.SQLEXPRESS\MSSQL\DATA\PhoneBook_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [PhoneBook] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [PhoneBook].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [PhoneBook] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [PhoneBook] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [PhoneBook] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [PhoneBook] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [PhoneBook] SET ARITHABORT OFF 
GO
ALTER DATABASE [PhoneBook] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [PhoneBook] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [PhoneBook] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [PhoneBook] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [PhoneBook] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [PhoneBook] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [PhoneBook] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [PhoneBook] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [PhoneBook] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [PhoneBook] SET  DISABLE_BROKER 
GO
ALTER DATABASE [PhoneBook] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [PhoneBook] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [PhoneBook] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [PhoneBook] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [PhoneBook] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [PhoneBook] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [PhoneBook] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [PhoneBook] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [PhoneBook] SET  MULTI_USER 
GO
ALTER DATABASE [PhoneBook] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [PhoneBook] SET DB_CHAINING OFF 
GO
ALTER DATABASE [PhoneBook] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [PhoneBook] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [PhoneBook] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [PhoneBook] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [PhoneBook] SET QUERY_STORE = OFF
GO
USE [PhoneBook]
GO
/****** Object:  Table [dbo].[__MigrationHistory]    Script Date: 29/01/2023 17:28:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__MigrationHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ContextKey] [nvarchar](300) NOT NULL,
	[Model] [varbinary](max) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK_dbo.__MigrationHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC,
	[ContextKey] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Companies]    Script Date: 29/01/2023 17:28:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Companies](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CompanyName] [nvarchar](max) NOT NULL,
	[RegistrationDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Companies] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[People]    Script Date: 29/01/2023 17:28:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FullName] [nvarchar](max) NOT NULL,
	[PhoneNumber] [nvarchar](max) NOT NULL,
	[Address] [nvarchar](max) NOT NULL,
	[CompanyId] [int] NOT NULL,
 CONSTRAINT [PK_dbo.People] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Index [IX_CompanyId]    Script Date: 29/01/2023 17:28:38 ******/
CREATE NONCLUSTERED INDEX [IX_CompanyId] ON [dbo].[People]
(
	[CompanyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_dbo.People_dbo.Companies_CompanyId] FOREIGN KEY([CompanyId])
REFERENCES [dbo].[Companies] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_dbo.People_dbo.Companies_CompanyId]
GO
USE [master]
GO
ALTER DATABASE [PhoneBook] SET  READ_WRITE 
GO
