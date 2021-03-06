SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[User]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[User](
	[account] [varchar](50) NOT NULL,
	[password] [varchar](50) NOT NULL,
	[id] [varchar](50) NULL,
	[ip] [varchar](15) NULL,
	[public_key] [text] NULL,
	[private_key] [text] NULL,
	[agent_type] [int] NOT NULL CONSTRAINT [DF_User_agent_type]  DEFAULT ((0)),
	[hub_id] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[account] ASC
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
