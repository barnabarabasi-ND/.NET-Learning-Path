USE [Insurances]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Counties]') AND type in (N'U'))
ALTER TABLE [dbo].[Counties] DROP CONSTRAINT IF EXISTS [FK_Counties_Countries]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cities]') AND type in (N'U'))
ALTER TABLE [dbo].[Cities] DROP CONSTRAINT IF EXISTS [FK_Cities_Counties]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Buildings]') AND type in (N'U'))
ALTER TABLE [dbo].[Buildings] DROP CONSTRAINT IF EXISTS [FK_Buildings_Clients]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Buildings]') AND type in (N'U'))
ALTER TABLE [dbo].[Buildings] DROP CONSTRAINT IF EXISTS [FK_Buildings_Cities]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RiskFactorConfigs]') AND type in (N'U'))
ALTER TABLE [dbo].[RiskFactorConfigs] DROP CONSTRAINT IF EXISTS [DF_RiskFactorConfigs_CreatedAt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RiskFactorConfigs]') AND type in (N'U'))
ALTER TABLE [dbo].[RiskFactorConfigs] DROP CONSTRAINT IF EXISTS [DF_RiskFactorConfigs_IsActive]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeeConfigs]') AND type in (N'U'))
ALTER TABLE [dbo].[FeeConfigs] DROP CONSTRAINT IF EXISTS [DF_FeeConfigs_CreatedAt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[FeeConfigs]') AND type in (N'U'))
ALTER TABLE [dbo].[FeeConfigs] DROP CONSTRAINT IF EXISTS [DF_FeeConfigs_IsActive]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Currencies]') AND type in (N'U'))
ALTER TABLE [dbo].[Currencies] DROP CONSTRAINT IF EXISTS [DF_Currencies_CreatedAt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Currencies]') AND type in (N'U'))
ALTER TABLE [dbo].[Currencies] DROP CONSTRAINT IF EXISTS [DF_Currencies_IsActive]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clients]') AND type in (N'U'))
ALTER TABLE [dbo].[Clients] DROP CONSTRAINT IF EXISTS [DF_Clients_CreatedAt]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Buildings]') AND type in (N'U'))
ALTER TABLE [dbo].[Buildings] DROP CONSTRAINT IF EXISTS [DF_Buildings_CreatedAt]
GO
/****** Object:  Index [UQ_Currencies_Code]    Script Date: 09/22/2026 12:26:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Currencies]') AND type in (N'U'))
ALTER TABLE [dbo].[Currencies] DROP CONSTRAINT IF EXISTS [UQ_Currencies_Code]
GO
/****** Object:  Index [UQ_Clients_IdentificationNumber]    Script Date: 09/22/2026 12:26:56 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Clients]') AND type in (N'U'))
ALTER TABLE [dbo].[Clients] DROP CONSTRAINT IF EXISTS [UQ_Clients_IdentificationNumber]
GO
/****** Object:  Table [dbo].[RiskFactorConfigs]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[RiskFactorConfigs]
GO
/****** Object:  Table [dbo].[FeeConfigs]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[FeeConfigs]
GO
/****** Object:  Table [dbo].[Currencies]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Currencies]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Countries]
GO
/****** Object:  Table [dbo].[Counties]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Counties]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Clients]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Cities]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 09/22/2026 12:26:56 ******/
DROP TABLE IF EXISTS [dbo].[Buildings]
GO
/****** Object:  Table [dbo].[Buildings]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Buildings](
	[BuildingId] [int] IDENTITY(1,1) NOT NULL,
	[ClientId] [int] NOT NULL,
	[CityId] [int] NOT NULL,
	[AddressStreet] [nvarchar](200) NOT NULL,
	[AddressStreetNumber] [nvarchar](20) NOT NULL,
	[ConstructionYear] [int] NOT NULL,
	[BuildingType] [int] NOT NULL,
	[NumberOfFloors] [int] NOT NULL,
	[SurfaceArea] [decimal](18, 2) NOT NULL,
	[InsuredValue] [decimal](18, 2) NOT NULL,
	[RiskIndicators] [nvarchar](1000) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Building] PRIMARY KEY CLUSTERED 
(
	[BuildingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Cities]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Cities](
	[CityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CountyId] [int] NOT NULL,
 CONSTRAINT [PK_Cities] PRIMARY KEY CLUSTERED 
(
	[CityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Clients]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Clients](
	[ClientId] [int] IDENTITY(1,1) NOT NULL,
	[ClientType] [int] NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[IdentificationNumber] [varchar](20) NOT NULL,
	[Email] [varchar](100) NULL,
	[Phone] [varchar](20) NULL,
	[Address] [nvarchar](200) NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Clients] PRIMARY KEY CLUSTERED 
(
	[ClientId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Counties]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Counties](
	[CountyId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[CountryId] [int] NOT NULL,
 CONSTRAINT [PK_Counties] PRIMARY KEY CLUSTERED 
(
	[CountyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Countries] PRIMARY KEY CLUSTERED 
(
	[CountryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Currencies]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Currencies](
	[CurrencyId] [int] IDENTITY(1,1) NOT NULL,
	[Code] [nvarchar](3) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[ExchangeRateToBase] [decimal](18, 6) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_Currencies] PRIMARY KEY CLUSTERED 
(
	[CurrencyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FeeConfigs]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeeConfigs](
	[FeeConfigId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[FeeType] [int] NOT NULL,
	[Percentage] [decimal](9, 4) NOT NULL,
	[EffectiveFrom] [date] NOT NULL,
	[EffectiveTo] [date] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_FeeConfigs] PRIMARY KEY CLUSTERED 
(
	[FeeConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RiskFactorConfigs]    Script Date: 09/22/2026 12:26:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RiskFactorConfigs](
	[RiskFactorConfigId] [int] IDENTITY(1,1) NOT NULL,
	[Level] [int] NOT NULL,
	[ReferenceId] [int] NULL,
	[AdjustmentPercentage] [decimal](9, 4) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAt] [datetime2](7) NOT NULL,
	[ModifiedAt] [datetime2](7) NULL,
 CONSTRAINT [PK_RiskFactorConfigs] PRIMARY KEY CLUSTERED 
(
	[RiskFactorConfigId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Cities] ON 
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (1, N'Cluj-Napoca', 1)
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (2, N'Turda', 1)
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (3, N'Bucuresti', 2)
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (4, N'Brasov', 3)
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (5, N'Timisoara', 4)
GO
INSERT [dbo].[Cities] ([CityId], [Name], [CountyId]) VALUES (6, N'Oradea', 5)
GO
SET IDENTITY_INSERT [dbo].[Cities] OFF
GO
SET IDENTITY_INSERT [dbo].[Counties] ON 
GO
INSERT [dbo].[Counties] ([CountyId], [Name], [CountryId]) VALUES (1, N'Cluj', 1)
GO
INSERT [dbo].[Counties] ([CountyId], [Name], [CountryId]) VALUES (2, N'Bucuresti', 1)
GO
INSERT [dbo].[Counties] ([CountyId], [Name], [CountryId]) VALUES (3, N'Brasov', 1)
GO
INSERT [dbo].[Counties] ([CountyId], [Name], [CountryId]) VALUES (4, N'Timis', 1)
GO
INSERT [dbo].[Counties] ([CountyId], [Name], [CountryId]) VALUES (5, N'Bihor', 1)
GO
SET IDENTITY_INSERT [dbo].[Counties] OFF
GO
SET IDENTITY_INSERT [dbo].[Countries] ON 
GO
INSERT [dbo].[Countries] ([CountryId], [Name]) VALUES (1, N'Romania')
GO
INSERT [dbo].[Countries] ([CountryId], [Name]) VALUES (2, N'Hungary')
GO
INSERT [dbo].[Countries] ([CountryId], [Name]) VALUES (3, N'Bulgaria')
GO
SET IDENTITY_INSERT [dbo].[Countries] OFF
GO
SET IDENTITY_INSERT [dbo].[Currencies] ON 
GO
INSERT [dbo].[Currencies] ([CurrencyId], [Code], [Name], [ExchangeRateToBase], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (1, N'RON', N'Romanian Leu', CAST(1.000000 AS Decimal(18, 6)), 1, CAST(N'2026-09-22T06:27:31.7252417' AS DateTime2), NULL)
GO
INSERT [dbo].[Currencies] ([CurrencyId], [Code], [Name], [ExchangeRateToBase], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (2, N'EUR', N'Euro', CAST(5.250000 AS Decimal(18, 6)), 1, CAST(N'2026-09-22T06:27:31.7252417' AS DateTime2), NULL)
GO
INSERT [dbo].[Currencies] ([CurrencyId], [Code], [Name], [ExchangeRateToBase], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (3, N'USD', N'US Dollar', CAST(4.400000 AS Decimal(18, 6)), 1, CAST(N'2026-09-22T06:27:31.7252417' AS DateTime2), NULL)
GO
INSERT [dbo].[Currencies] ([CurrencyId], [Code], [Name], [ExchangeRateToBase], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (4, N'GBP', N'British Pound', CAST(5.900000 AS Decimal(18, 6)), 0, CAST(N'2026-09-22T06:27:31.7252417' AS DateTime2), NULL)
GO
INSERT [dbo].[Currencies] ([CurrencyId], [Code], [Name], [ExchangeRateToBase], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (5, N'AAB', N'currency updated', CAST(3.000000 AS Decimal(18, 6)), 1, CAST(N'2026-09-22T08:04:56.7126011' AS DateTime2), CAST(N'2026-09-22T08:06:04.6855569' AS DateTime2))
GO
SET IDENTITY_INSERT [dbo].[Currencies] OFF
GO
SET IDENTITY_INSERT [dbo].[FeeConfigs] ON 
GO
INSERT [dbo].[FeeConfigs] ([FeeConfigId], [Name], [FeeType], [Percentage], [EffectiveFrom], [EffectiveTo], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (1, N'Standard broker fee', 1, CAST(5.0000 AS Decimal(9, 4)), CAST(N'2026-01-01' AS Date), NULL, 1, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
INSERT [dbo].[FeeConfigs] ([FeeConfigId], [Name], [FeeType], [Percentage], [EffectiveFrom], [EffectiveTo], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (2, N'Risk adjustment fee', 2, CAST(2.0000 AS Decimal(9, 4)), CAST(N'2026-01-01' AS Date), NULL, 1, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
INSERT [dbo].[FeeConfigs] ([FeeConfigId], [Name], [FeeType], [Percentage], [EffectiveFrom], [EffectiveTo], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (3, N'Administration fee', 3, CAST(1.5000 AS Decimal(9, 4)), CAST(N'2025-01-01' AS Date), CAST(N'2025-12-31' AS Date), 0, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[FeeConfigs] OFF
GO
SET IDENTITY_INSERT [dbo].[RiskFactorConfigs] ON 
GO
INSERT [dbo].[RiskFactorConfigs] ([RiskFactorConfigId], [Level], [ReferenceId], [AdjustmentPercentage], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (1, 2, 1, CAST(3.0000 AS Decimal(9, 4)), 1, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
INSERT [dbo].[RiskFactorConfigs] ([RiskFactorConfigId], [Level], [ReferenceId], [AdjustmentPercentage], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (2, 3, 3, CAST(5.0000 AS Decimal(9, 4)), 1, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
INSERT [dbo].[RiskFactorConfigs] ([RiskFactorConfigId], [Level], [ReferenceId], [AdjustmentPercentage], [IsActive], [CreatedAt], [ModifiedAt]) VALUES (3, 4, 1, CAST(2.0000 AS Decimal(9, 4)), 1, CAST(N'2026-09-14T14:27:49.8579905' AS DateTime2), NULL)
GO
SET IDENTITY_INSERT [dbo].[RiskFactorConfigs] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Clients_IdentificationNumber]    Script Date: 09/22/2026 12:26:56 ******/
ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [UQ_Clients_IdentificationNumber] UNIQUE NONCLUSTERED 
(
	[IdentificationNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ_Currencies_Code]    Script Date: 09/22/2026 12:26:56 ******/
ALTER TABLE [dbo].[Currencies] ADD  CONSTRAINT [UQ_Currencies_Code] UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Buildings] ADD  CONSTRAINT [DF_Buildings_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Clients] ADD  CONSTRAINT [DF_Clients_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Currencies] ADD  CONSTRAINT [DF_Currencies_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Currencies] ADD  CONSTRAINT [DF_Currencies_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[FeeConfigs] ADD  CONSTRAINT [DF_FeeConfigs_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[FeeConfigs] ADD  CONSTRAINT [DF_FeeConfigs_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[RiskFactorConfigs] ADD  CONSTRAINT [DF_RiskFactorConfigs_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[RiskFactorConfigs] ADD  CONSTRAINT [DF_RiskFactorConfigs_CreatedAt]  DEFAULT (sysutcdatetime()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD  CONSTRAINT [FK_Buildings_Cities] FOREIGN KEY([CityId])
REFERENCES [dbo].[Cities] ([CityId])
GO
ALTER TABLE [dbo].[Buildings] CHECK CONSTRAINT [FK_Buildings_Cities]
GO
ALTER TABLE [dbo].[Buildings]  WITH CHECK ADD  CONSTRAINT [FK_Buildings_Clients] FOREIGN KEY([ClientId])
REFERENCES [dbo].[Clients] ([ClientId])
GO
ALTER TABLE [dbo].[Buildings] CHECK CONSTRAINT [FK_Buildings_Clients]
GO
ALTER TABLE [dbo].[Cities]  WITH CHECK ADD  CONSTRAINT [FK_Cities_Counties] FOREIGN KEY([CountyId])
REFERENCES [dbo].[Counties] ([CountyId])
GO
ALTER TABLE [dbo].[Cities] CHECK CONSTRAINT [FK_Cities_Counties]
GO
ALTER TABLE [dbo].[Counties]  WITH CHECK ADD  CONSTRAINT [FK_Counties_Countries] FOREIGN KEY([CountryId])
REFERENCES [dbo].[Countries] ([CountryId])
GO
ALTER TABLE [dbo].[Counties] CHECK CONSTRAINT [FK_Counties_Countries]
GO
