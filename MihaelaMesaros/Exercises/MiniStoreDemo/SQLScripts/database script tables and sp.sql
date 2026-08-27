USE [MiniStoreDemo]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Products]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](200) NOT NULL,
	[ProductDescription] [nvarchar](500) NULL,
	[ProductPrice] [decimal](10, 2) NOT NULL,
	[CategoryId] [int] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Roles]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Roles](
	[RoleId] [smallint] IDENTITY(1,1) NOT NULL,
	[Role] [varchar](10) NULL,
 CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
/****** Object:  Table [dbo].[Users]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](100) NOT NULL,
	[PasswordHash] [nvarchar](500) NOT NULL,
	[Role] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Users_Username] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_Products_ProductName_CategoryId]    Script Date: 08/27/2026 08:22:03 ******/
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND name = N'UX_Products_ProductName_CategoryId')
CREATE UNIQUE NONCLUSTERED INDEX [UX_Products_ProductName_CategoryId] ON [dbo].[Products]
(
	[ProductName] ASC,
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Products__IsActi__4CA06362]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__IsActi__4CA06362]  DEFAULT ((1)) FOR [IsActive]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Products__Create__4D94879B]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Products] ADD  CONSTRAINT [DF__Products__Create__4D94879B]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_IsActive]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_IsActive]  DEFAULT ((1)) FOR [IsActive]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Users_CreatedAt]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
END
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Products_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Products_Categories]') AND parent_object_id = OBJECT_ID(N'[dbo].[Products]'))
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Users_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [CK_Users_Role] CHECK  (([Role]='User' OR [Role]='Admin'))
GO
IF  EXISTS (SELECT * FROM sys.check_constraints WHERE object_id = OBJECT_ID(N'[dbo].[CK_Users_Role]') AND parent_object_id = OBJECT_ID(N'[dbo].[Users]'))
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [CK_Users_Role]
GO
/****** Object:  StoredProcedure [dbo].[Category_CheckExists]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Category_CheckExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Category_CheckExists] AS' 
END
GO
-- =============================================
-- Author:		Mihaela Mesaros
-- Create date: 26.08.2026
-- Description:	Check if category exists, by Id.
-- =============================================
/*
declare @categoryExists bit
exec Category_CheckExists @CategoryId=2, @outCategoryExists = @categoryExists output
select @categoryExists
*/
ALTER   PROCEDURE [dbo].[Category_CheckExists]
    @CategoryId int,
    @outCategoryExists bit output
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS (SELECT TOP 1 1 FROM Categories WHERE CategoryId = @CategoryId)
        SET @outCategoryExists = 1
    ELSE 
        SET @outCategoryExists = 0
    
END
GO
/****** Object:  StoredProcedure [dbo].[Product_CheckAlreadyExists]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product_CheckAlreadyExists]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Product_CheckAlreadyExists] AS' 
END
GO
-- =============================================
-- Author:		Mihaela Mesaros
-- Create date: 26.08.2026
-- Description:	Check if already exists a product with the same name in the same category, excepting current product.
-- =============================================
/*
declare @productExists bit
exec Product_CheckAlreadyExists @ProductName='Dell UltraSharp 27', @ExcludeProductId=null, @CategoryId=2, @outProductExists = @productExists output
select @productExists
*/
ALTER   PROCEDURE [dbo].[Product_CheckAlreadyExists]
    @ProductName nvarchar(200),
    @ExcludeProductId int,
    @CategoryId int,
    @outProductExists bit output
AS
BEGIN
	SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Products WHERE ProductName = @ProductName AND CategoryId = @CategoryId AND (@ExcludeProductId IS NULL OR ProductId <> @ExcludeProductId))
        SET @outProductExists = 1
    ELSE 
        SET @outProductExists = 0
    
END
GO
/****** Object:  StoredProcedure [dbo].[Product_GetAll]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product_GetAll]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Product_GetAll] AS' 
END
GO
-- =============================================
-- Author:		Mihaela Mesaros
-- Create date: 26.08.2026
-- Description:	Get product by Id.
-- =============================================
/*
exec Product_GetAll @CategoryId=null, @IsActive=1, @Keyword='laptop', @PageNumber=1, @PageSize=2
*/
ALTER   PROCEDURE [dbo].[Product_GetAll]
    @CategoryId int = NULL,
    @IsActive bit = NULL,
    @Keyword nvarchar(200) = NULL,
    @PageNumber int = 1,
    @PageSize int = 10
AS
BEGIN
	SET NOCOUNT ON;

    SET @Keyword = NULLIF(TRIM(@Keyword), '');

    SELECT ProductId, ProductName, ProductDescription, ProductPrice, CategoryId, IsActive, CreatedAt, ModifiedAt
    FROM Products
    WHERE
        (@CategoryId IS NULL OR CategoryId = @CategoryId)
        AND (@IsActive IS NULL OR IsActive = @IsActive)
        AND (
            @Keyword IS NULL
            OR ProductName LIKE '%' + @Keyword + '%'
            OR ProductDescription LIKE '%' + @Keyword + '%'
        )
    ORDER BY ProductName
    OFFSET (@PageNumber - 1) * @PageSize ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO
/****** Object:  StoredProcedure [dbo].[Product_GetById]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Product_GetById]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[Product_GetById] AS' 
END
GO
-- =============================================
-- Author:		Mihaela Mesaros
-- Create date: 26.08.2026
-- Description:	Get product by Id.
-- =============================================
/*
exec Product_GetById 10
*/
ALTER   PROCEDURE [dbo].[Product_GetById]
    @ProductId int
AS
BEGIN
	SET NOCOUNT ON;

    SELECT ProductId, ProductName, ProductDescription, ProductPrice, CategoryId, IsActive, CreatedAt, ModifiedAt
    FROM Products
    WHERE ProductId = @ProductId
END
GO
/****** Object:  StoredProcedure [dbo].[User_GetByUsername]    Script Date: 08/27/2026 08:22:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User_GetByUsername]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'CREATE PROCEDURE [dbo].[User_GetByUsername] AS' 
END
GO
-- =============================================
-- Author:		Mihaela Mesaros
-- Create date: 26.08.2026
-- Description:	Get user by username.
-- =============================================
/*
exec User_GetByUsername 'admin'
*/
ALTER   PROCEDURE [dbo].[User_GetByUsername]
    @Username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM Users
    WHERE Username = @Username;
END
GO
