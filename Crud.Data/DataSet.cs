using Crud.Data.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crud.Data
{
    public class DataSet : IBookRepository, IUserRepository
    {
        private readonly IDbConnection _dbConnection;

        public DataSet(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task InitializeDatabase()
        {
            await CheckDatabase();
            await CheckUserTable();
            await CheckBookTable();
        }

        public async Task<BookDto> CreateBook(BookDto book)
        {
            const string sql = @"INSERT INTO [dbo].[Book]
                                       ([Title]
                                       ,[Author]
                                       ,[Description]
                                       ,[ReleaseDate])
                                 VALUES
                                       (@Title
                                       ,@Author
                                       ,@Description
                                       ,@ReleaseDate);
                                SELECT scope_identity()";
            var id = await _dbConnection.QueryFirstAsync<int>(sql, new
            {
                book.Title,
                book.Author,
                book.Description,
                book.ReleaseDate
            });
            book.IdBook = id;
            return book;

        }

        public async Task DeleteBook(int id)
        {
            const string sql = @"DELETE FROM [dbo].[Book]
                                WHERE [IdBook] = @IdBook";
            await _dbConnection.ExecuteAsync(sql, new { IdBook = id });

        }

        public async Task<BookDto> GetBook(int id)
        {
            const string sql = @"SELECT [IdBook]
                                  ,[Title]
                                  ,[Author]
                                  ,[Description]
                                  ,[ReleaseDate]
                              FROM [CrudDb].[dbo].[Book]
                              WHERE [IdBook] = @IdBook";
            return await _dbConnection.QueryFirstOrDefaultAsync<BookDto>(sql, new
            {
                IdBook = id
            });
        }

        public async Task<List<BookDto>> GetBooks()
        {
            const string sql = @"SELECT [IdBook]
                                  ,[Title]
                                  ,[Author]
                                  ,[Description]
                                  ,[ReleaseDate]
                              FROM [CrudDb].[dbo].[Book]";
            return (await _dbConnection.QueryAsync<BookDto>(sql)).ToList();
        }

        public async Task UpdateBook(BookDto book)
        {

            const string sql = @"UPDATE[dbo].[Book]
                               SET [Title] = @Title
                                  ,[Author] = @Author
                                  ,[Description] = @Description
                                  ,[ReleaseDate] = @ReleaseDate
                               WHERE [IdBook] = @IdBook";
            await _dbConnection.ExecuteAsync(sql, new
            {
                book.IdBook,
                book.Title,
                book.Author,
                book.Description,
                book.ReleaseDate
            });
        }

        public async Task<UserDto> CreateUser(UserDto user)
        {
            const string sql = @"INSERT INTO [dbo].[User]
                                    ([Email]
                                    ,[FirstName]
                                    ,[LastName]
                                    ,[Password])
                                VALUES
                                    (@Email
                                    ,@FirstName
                                    ,@LastName
                                    ,@Password);
                                SELECT scope_identity()";
            var id = await _dbConnection.QueryFirstAsync<int>(sql, new
            {
                user.Email,
                user.FirstName,
                user.LastName,
                user.Password
            });
            user.IdUser = id;
            user.Password = string.Empty;

            return user;

        }

        public async Task<UserDto> GetUser(string email, string password)
        {
            const string sql = @"SELECT [Email]
                                       ,[FirstName]
                                       ,[LastName]
                                       ,[Password]
                                 FROM [dbo].[User]
	                             WHERE [Email] = @Email 
		                            AND [Password] = @Password";
            return await _dbConnection.QueryFirstOrDefaultAsync<UserDto>(sql, new
            {
                Email = email,
                Password = password
            });
        }

        private async Task CheckDatabase()
        {
            const string sql = @"
USE [master]
IF NOT EXISTS (SELECT name FROM master.sys.databases WHERE name = N'CrudDb')
BEGIN
	/****** Object:  Database [CrudDb]    Script Date: 22/08/2023 3:57:44 p. m. ******/
	CREATE DATABASE [CrudDb]
	 CONTAINMENT = NONE
	 ON  PRIMARY 
	( NAME = N'CrudDb', FILENAME = N'/var/opt/mssql/data/CrudDb.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
	 LOG ON 
	( NAME = N'CrudDb_log', FILENAME = N'/var/opt/mssql/data/CrudDb_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
	 WITH CATALOG_COLLATION = DATABASE_DEFAULT
	

	IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
	begin
	EXEC [CrudDb].[dbo].[sp_fulltext_database] @action = 'enable'
	end

	ALTER DATABASE [CrudDb] SET ANSI_NULL_DEFAULT OFF 
	ALTER DATABASE [CrudDb] SET ANSI_NULLS OFF 
	ALTER DATABASE [CrudDb] SET ANSI_PADDING OFF 
	ALTER DATABASE [CrudDb] SET ANSI_WARNINGS OFF 
	ALTER DATABASE [CrudDb] SET ARITHABORT OFF 
	ALTER DATABASE [CrudDb] SET AUTO_CLOSE OFF 
	ALTER DATABASE [CrudDb] SET AUTO_SHRINK OFF 
	ALTER DATABASE [CrudDb] SET AUTO_UPDATE_STATISTICS ON 
	ALTER DATABASE [CrudDb] SET CURSOR_CLOSE_ON_COMMIT OFF 
	ALTER DATABASE [CrudDb] SET CURSOR_DEFAULT  GLOBAL 
	ALTER DATABASE [CrudDb] SET CONCAT_NULL_YIELDS_NULL OFF 
	ALTER DATABASE [CrudDb] SET NUMERIC_ROUNDABORT OFF 
	ALTER DATABASE [CrudDb] SET QUOTED_IDENTIFIER OFF 
	ALTER DATABASE [CrudDb] SET RECURSIVE_TRIGGERS OFF 
	ALTER DATABASE [CrudDb] SET  DISABLE_BROKER 
	ALTER DATABASE [CrudDb] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
	ALTER DATABASE [CrudDb] SET DATE_CORRELATION_OPTIMIZATION OFF 
	ALTER DATABASE [CrudDb] SET TRUSTWORTHY OFF 
	ALTER DATABASE [CrudDb] SET ALLOW_SNAPSHOT_ISOLATION OFF 
	ALTER DATABASE [CrudDb] SET PARAMETERIZATION SIMPLE 
	ALTER DATABASE [CrudDb] SET READ_COMMITTED_SNAPSHOT OFF 
	ALTER DATABASE [CrudDb] SET HONOR_BROKER_PRIORITY OFF 
	ALTER DATABASE [CrudDb] SET RECOVERY FULL 
	ALTER DATABASE [CrudDb] SET  MULTI_USER 
	ALTER DATABASE [CrudDb] SET PAGE_VERIFY CHECKSUM  
	ALTER DATABASE [CrudDb] SET DB_CHAINING OFF 
	ALTER DATABASE [CrudDb] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
	ALTER DATABASE [CrudDb] SET TARGET_RECOVERY_TIME = 60 SECONDS 
	ALTER DATABASE [CrudDb] SET DELAYED_DURABILITY = DISABLED 
	ALTER DATABASE [CrudDb] SET ACCELERATED_DATABASE_RECOVERY = OFF  
	ALTER DATABASE [CrudDb] SET QUERY_STORE = OFF
	ALTER DATABASE [CrudDb] SET  READ_WRITE 
END";
            await _dbConnection.ExecuteAsync(sql);

        }

        private async Task CheckUserTable()
        {
            const string sql = @"
USE [CrudDb]
IF( OBJECT_ID('dbo.User', 'U') IS  NULL )
BEGIN
	/****** Object:  Table [dbo].[User]    Script Date: 23/08/2023 7:13:16 p. m. ******/
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[User](
		[IdUser] [int] IDENTITY(1,1) NOT NULL,
		[Email] [varchar](1000) NOT NULL,
		[FirstName] [varchar](1000) NOT NULL,
		[LastName] [varchar](1000) NOT NULL,
		[Password] [varchar](1000) NOT NULL,
	 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
	(
		[IdUser] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY]

END";
            await _dbConnection.ExecuteAsync(sql);

        }

        private async Task CheckBookTable()
        {
            const string sql = @"
USE [CrudDb]
IF( OBJECT_ID('dbo.Book', 'U') IS  NULL )
BEGIN
	
	/****** Object:  Table [dbo].[Book]    Script Date: 23/08/2023 7:07:43 p. m. ******/
	SET ANSI_NULLS ON
	SET QUOTED_IDENTIFIER ON
	CREATE TABLE [dbo].[Book](
		[IdBook] [int] IDENTITY(1,1) NOT NULL,
		[Title] [varchar](1000) NOT NULL,
		[Author] [varchar](500) NOT NULL,
		[Description] [varchar](max) NOT NULL,
		[ReleaseDate] [datetime2](7) NULL,
	 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
	(
		[IdBook] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
	
END";
            await _dbConnection.ExecuteAsync(sql);

        }

    }
}
