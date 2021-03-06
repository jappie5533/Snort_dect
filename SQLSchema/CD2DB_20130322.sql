SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[CmdPrivilege]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[CmdPrivilege](
	[cid] [int] IDENTITY(1,1) NOT NULL,
	[command] [varchar](50) NOT NULL,
	[privilege] [int] NOT NULL,
	[note] [varchar](200) NULL,
 CONSTRAINT [PK_CmdPrivilege] PRIMARY KEY CLUSTERED 
(
	[cid] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Hub]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Hub](
	[id] [bigint] NOT NULL,
	[ip] [varchar](15) NOT NULL,
	[public_key] [text] NOT NULL,
	[private_key] [text] NOT NULL,
	[timestamp] [int] NOT NULL,
	[account] [varchar](50) NOT NULL,
	[city] [varchar](20) NULL,
	[country] [varchar](20) NULL,
	[connections] [text] NULL,
 CONSTRAINT [PK_Hub] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Log]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Log](
	[src] [varchar](50) NOT NULL,
	[timestamp] [int] NOT NULL,
	[account] [varchar](50) NOT NULL,
	[data_length] [bigint] NOT NULL,
	[hub_id] [bigint] NOT NULL,
	[hub_account] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[src] ASC,
	[timestamp] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GeoIPBlocks]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GeoIPBlocks](
	[startIpNum] [numeric](18, 0) NULL,
	[endIpNum] [numeric](18, 0) NULL,
	[locId] [numeric](18, 0) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[HubLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[HubLog](
	[id] [bigint] NOT NULL,
	[ip] [varchar](15) NOT NULL,
	[start_timestamp] [int] NOT NULL,
	[end_timestamp] [int] NULL,
	[account] [varchar](50) NOT NULL,
	[city] [varchar](20) NULL,
	[country] [nchar](20) NULL,
 CONSTRAINT [PK_HubLog] PRIMARY KEY CLUSTERED 
(
	[start_timestamp] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GeoIPLocation]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GeoIPLocation](
	[locId] [nvarchar](50) NULL,
	[country] [nvarchar](50) NULL,
	[region] [nvarchar](50) NULL,
	[city] [nvarchar](50) NULL,
	[postalCode] [nvarchar](50) NULL,
	[latitude] [nvarchar](50) NULL,
	[longitude] [nvarchar](50) NULL,
	[metroCode] [nvarchar](50) NULL,
	[areaCode] [nvarchar](50) NULL
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[uid] [int] IDENTITY(1,1) NOT NULL,
	[account] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[email] [varchar](100) NOT NULL,
	[id] [bigint] NULL CONSTRAINT [DF_User_id]  DEFAULT ((0)),
	[ip] [varchar](15) NULL,
	[public_key] [text] NULL,
	[private_key] [text] NULL,
	[agent_type] [int] NOT NULL CONSTRAINT [DF_User_agent_type]  DEFAULT ((0)),
	[hub_id] [bigint] NULL,
	[isAdm] [bit] NOT NULL CONSTRAINT [DF_User_isAdm]  DEFAULT ((0)),
 CONSTRAINT [PK_User_1] PRIMARY KEY CLUSTERED 
(
	[uid] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
