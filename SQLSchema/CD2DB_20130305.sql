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
 CONSTRAINT [PK_Hub] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
