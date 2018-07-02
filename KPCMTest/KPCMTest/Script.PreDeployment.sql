/*
 Pre-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be executed before the build script.	
 Use SQLCMD syntax to include a file in the pre-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the pre-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

CREATE TABLE [dbo].[Assertions_Client](
	[Assertion_ID] [int] IDENTITY(1,1) NOT NULL,
	[WS_ID] [int] NOT NULL,
	[Method_ID] [int] NOT NULL,
	[Rule_ID] [int] NOT NULL,
	[Rule_Item_ID] [int] NOT NULL,
	[Resource_ID] [int] NOT NULL,
	[Weight] [float] NULL,
	[Mandatory_Flag] [bit] NULL,
	[target_ws_id] [int] NULL,
 CONSTRAINT [PK_Assertions5] PRIMARY KEY CLUSTERED 
(
	[Assertion_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Assertions_WebService](
	[Assertion_ID] [int] IDENTITY(1,1) NOT NULL,
	[WS_ID] [int] NOT NULL,
	[Method_ID] [int] NOT NULL,
	[Rule_ID] [int] NOT NULL,
	[Rule_Item_ID] [int] NOT NULL,
	[Resource_ID] [int] NOT NULL,
	[Weight] [float] NULL,
	[Mandatory_Flag] [bit] NULL,
 CONSTRAINT [PK_Assertions4] PRIMARY KEY CLUSTERED 
(
	[Assertion_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Domain](
	[Domain_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[Domain_SubsumptionRelationships](
	[Subsumed_Domain_ID] [int] NOT NULL,
	[Subsuming_Domain_ID] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[Level](
	[Level_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[PrivacyRuleSet](
	[Rule_ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[PrivacyRuleSetItems](
	[Rule_Item_ID] [int] IDENTITY(1,1) NOT NULL,
	[Rule_ID] [int] NOT NULL,
	[Topic_ID] [int] NOT NULL,
	[Level_ID] [int] NOT NULL,
	[Domain_ID] [int] NOT NULL,
	[Scope_ID] [int] NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[PrivacyRuleSetItems_Client](
	[WS_ID] [int] NOT NULL,
	[Rule_ID] [int] NOT NULL,
	[Rule_Item_ID] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[PrivacyRuleSetItems_WebService](
	[WS_ID] [int] NOT NULL,
	[Rule_ID] [int] NOT NULL,
	[Rule_Item_ID] [int] NOT NULL
) ON [PRIMARY]



CREATE TABLE [dbo].[Scope](
	[Scope_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL
) ON [PRIMARY]


CREATE TABLE [dbo].[Topic](
	[Topic_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[WSCL_Transitions](
	[WS_ID] [int] NOT NULL,
	[Source_Method_ID] [int] NOT NULL,
	[Destination_Method_ID] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[WSCL_Transitions_AllPossibleRoutes](
	[WS_ID] [int] NOT NULL,
	[Source_Method_ID] [int] NOT NULL,
	[Destination_Method_ID] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[WSCL_Transitions_KAnonymity](
	[WS_ID] [int] NOT NULL,
	[source_method_id] [int] NOT NULL,
	[destination_method_id] [int] NOT NULL,
	[KAnon] [int] NOT NULL,
	[Scope_Desc] [nvarchar](20) NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[WSCL_Transitions_PrivacyRuleSetItem_WebService_RowsToAdd](
	[WS_ID] [int] NOT NULL,
	[rule_id] [int] NOT NULL,
	[rule_item_id] [int] NOT NULL
) ON [PRIMARY]

CREATE TABLE [dbo].[WSMethodParameters](
	[Parameter_ID] [int] IDENTITY(1,1) NOT NULL,
	[Method_ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Type] [nchar](10) NOT NULL,
	[RefParam] [bit] NOT NULL,
 CONSTRAINT [PK_WSMethodParameters] PRIMARY KEY CLUSTERED 
(
	[Parameter_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[WSMethods](
	[Method_ID] [int] IDENTITY(1,1) NOT NULL,
	[WS_ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WSMethods] PRIMARY KEY CLUSTERED 
(
	[Method_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[WSResources](
	[Resource_ID] [int] IDENTITY(1,1) NOT NULL,
	[WS_ID] [int] NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_WSResources] PRIMARY KEY CLUSTERED 
(
	[Resource_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Client_WSProxyNames](
	[Name] [nvarchar](50) NOT NULL
) ON [PRIMARY]



CREATE TABLE [dbo].[ClientAndServices](
	[WS_ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](20) NOT NULL,
	[Priv_Match_Threshold] [decimal](4, 3) NULL,
 CONSTRAINT [PK_ClientAndServices] PRIMARY KEY CLUSTERED 
(
	[WS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[GUID_IncentivesOffered](
	[GUID] [nvarchar](40) NOT NULL,
	[Incentive_ID] [int] NOT NULL
) ON [PRIMARY]



CREATE TABLE [dbo].[Incentives](
	[Incentive_ID] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Rank] [int] NOT NULL
) ON [PRIMARY]


SELECT * FROM information_schema.tables

