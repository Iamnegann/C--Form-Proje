/****** Object:  Database [bitirmepr]    Script Date: 22.04.2025 14:28:36 ******/
CREATE DATABASE [bitirmepr]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'bitirmepr', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bitirmepr.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'bitirmepr_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.SQLEXPRESS\MSSQL\DATA\bitirmepr_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [bitirmepr] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [bitirmepr].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [bitirmepr] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [bitirmepr] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [bitirmepr] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [bitirmepr] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [bitirmepr] SET ARITHABORT OFF 
GO
ALTER DATABASE [bitirmepr] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [bitirmepr] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [bitirmepr] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [bitirmepr] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [bitirmepr] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [bitirmepr] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [bitirmepr] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [bitirmepr] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [bitirmepr] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [bitirmepr] SET  DISABLE_BROKER 
GO
ALTER DATABASE [bitirmepr] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [bitirmepr] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [bitirmepr] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [bitirmepr] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [bitirmepr] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [bitirmepr] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [bitirmepr] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [bitirmepr] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [bitirmepr] SET  MULTI_USER 
GO
ALTER DATABASE [bitirmepr] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [bitirmepr] SET DB_CHAINING OFF 
GO
ALTER DATABASE [bitirmepr] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [bitirmepr] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [bitirmepr] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [bitirmepr] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [bitirmepr] SET QUERY_STORE = ON
GO
ALTER DATABASE [bitirmepr] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
/****** Object:  Table [dbo].[Kullanicilar]    Script Date: 22.04.2025 14:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kullanicilar](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciAdi] [nvarchar](50) NULL,
	[Sifre] [nvarchar](50) NULL,
	[isim] [nvarchar](50) NULL,
	[soyisim] [nvarchar](50) NULL,
	[telefon] [nvarchar](50) NULL,
	[Rol] [nvarchar](10) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sepet]    Script Date: 22.04.2025 14:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sepet](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciId] [int] NULL,
	[UrunId] [int] NULL,
	[Adet] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Siparisler]    Script Date: 22.04.2025 14:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Siparisler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[KullaniciId] [int] NULL,
	[UrunId] [int] NULL,
	[Adet] [int] NULL,
	[ToplamFiyat] [decimal](18, 2) NULL,
	[SiparisTarihi] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Urunler]    Script Date: 22.04.2025 14:28:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Urunler](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UrunAdi] [nvarchar](100) NULL,
	[Fiyat] [decimal](10, 2) NULL,
	[Stok] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Kullanicilar] ON 

INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (1, N'Admin', N'123456', N'Mücahit', N'Kurt', N'05570570057', N'admin')
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (2, N'efegmsts', N'6161', N'Efe', N'Gümüstas', N'05560030012', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (4, N'emreckrk', N'2828', N'Emre', N'Çakrak', N'02120341212', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (5, N'emir', N'9999', N'Emir', N'Hasan', N'03922333434', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (6, N'omerfb', N'omer1907', N'Ömer', N'Aydoğdu', N'05050563222', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (7, N'negann', N'0007TR', N'Berkay', N'Kılıç', N'02129999999', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (8, N'mchtkrt', N'575757', N'Mücahit', N'Kurt', N'05550550055', NULL)
INSERT [dbo].[Kullanicilar] ([Id], [KullaniciAdi], [Sifre], [isim], [soyisim], [telefon], [Rol]) VALUES (10, N'talısca94', N'0094', N'anderson', N'talısca', N'05940949494', NULL)
SET IDENTITY_INSERT [dbo].[Kullanicilar] OFF
GO
SET IDENTITY_INSERT [dbo].[Sepet] ON 

INSERT [dbo].[Sepet] ([Id], [KullaniciId], [UrunId], [Adet]) VALUES (39, 4, 3, 1907)
SET IDENTITY_INSERT [dbo].[Sepet] OFF
GO
SET IDENTITY_INSERT [dbo].[Siparisler] ON 

INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (1, 4, 1, 3, CAST(240.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:46:37.520' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (2, 4, 9, 3, CAST(750.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:46:37.540' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (3, 4, 5, 4, CAST(600.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:46:37.543' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (4, 4, 2, 3, CAST(450.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:49:19.483' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (5, 4, 6, 6, CAST(120.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:49:19.483' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (6, 4, 8, 90, CAST(9000.00 AS Decimal(18, 2)), CAST(N'2025-04-17T18:49:19.487' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (7, 5, 5, 6, CAST(900.00 AS Decimal(18, 2)), CAST(N'2025-04-17T19:01:39.070' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (8, 5, 9, 16, CAST(4000.00 AS Decimal(18, 2)), CAST(N'2025-04-17T19:01:39.070' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (9, 4, 6, 8, CAST(160.00 AS Decimal(18, 2)), CAST(N'2025-04-17T19:16:41.430' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (10, 8, 6, 10, CAST(200.00 AS Decimal(18, 2)), CAST(N'2025-04-20T16:07:26.470' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (11, 8, 9, 100, CAST(25000.00 AS Decimal(18, 2)), CAST(N'2025-04-20T16:07:26.473' AS DateTime))
INSERT [dbo].[Siparisler] ([Id], [KullaniciId], [UrunId], [Adet], [ToplamFiyat], [SiparisTarihi]) VALUES (12, 10, 6, 20, CAST(400.00 AS Decimal(18, 2)), CAST(N'2025-04-20T18:22:01.600' AS DateTime))
SET IDENTITY_INSERT [dbo].[Siparisler] OFF
GO
SET IDENTITY_INSERT [dbo].[Urunler] ON 

INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (1, N'Nutuk', CAST(80.00 AS Decimal(10, 2)), 10)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (2, N'Kozmos', CAST(150.00 AS Decimal(10, 2)), 10)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (3, N'Fenerbahçe', CAST(12.00 AS Decimal(10, 2)), 1907)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (5, N'suç ve ceza', CAST(150.00 AS Decimal(10, 2)), 20)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (6, N'Saftirik', CAST(20.00 AS Decimal(10, 2)), 100)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (8, N'Bilgisayara Giriş', CAST(100.00 AS Decimal(10, 2)), 200)
INSERT [dbo].[Urunler] ([Id], [UrunAdi], [Fiyat], [Stok]) VALUES (9, N'Çanakkale', CAST(250.00 AS Decimal(10, 2)), 800)
SET IDENTITY_INSERT [dbo].[Urunler] OFF
GO
ALTER TABLE [dbo].[Siparisler] ADD  DEFAULT (getdate()) FOR [SiparisTarihi]
GO
ALTER TABLE [dbo].[Sepet]  WITH CHECK ADD FOREIGN KEY([KullaniciId])
REFERENCES [dbo].[Kullanicilar] ([Id])
GO
ALTER TABLE [dbo].[Sepet]  WITH CHECK ADD FOREIGN KEY([UrunId])
REFERENCES [dbo].[Urunler] ([Id])
GO
ALTER TABLE [dbo].[Siparisler]  WITH CHECK ADD FOREIGN KEY([KullaniciId])
REFERENCES [dbo].[Kullanicilar] ([Id])
GO
ALTER TABLE [dbo].[Siparisler]  WITH CHECK ADD FOREIGN KEY([UrunId])
REFERENCES [dbo].[Urunler] ([Id])
GO
ALTER DATABASE [bitirmepr] SET  READ_WRITE 
GO
