USE [TripYari]
GO
ALTER TABLE [blog].[post_tag] DROP CONSTRAINT [fk_pt_tag]
GO
ALTER TABLE [blog].[post_tag] DROP CONSTRAINT [fk_pt_post]
GO
ALTER TABLE [blog].[post_meta] DROP CONSTRAINT [fk_meta_post]
GO
ALTER TABLE [blog].[post_comment] DROP CONSTRAINT [fk_comment_post]
GO
ALTER TABLE [blog].[post_comment] DROP CONSTRAINT [fk_comment_parent]
GO
ALTER TABLE [blog].[post_category] DROP CONSTRAINT [fk_pc_post]
GO
ALTER TABLE [blog].[post_category] DROP CONSTRAINT [fk_pc_category]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [fk_post_parent]
GO
ALTER TABLE [blog].[category] DROP CONSTRAINT [fk_category_parent]
GO
ALTER TABLE [trip].[district] DROP CONSTRAINT [DF__district__state___7B5B524B]
GO
ALTER TABLE [trip].[city] DROP CONSTRAINT [DF__city__status__7F2BE32F]
GO
ALTER TABLE [trip].[city] DROP CONSTRAINT [DF__city__state_id__7E37BEF6]
GO
ALTER TABLE [trip].[city] DROP CONSTRAINT [DF__city__district_i__7D439ABD]
GO
ALTER TABLE [blog].[tag] DROP CONSTRAINT [DF__tag__metaTitle__73BA3083]
GO
ALTER TABLE [blog].[post_comment] DROP CONSTRAINT [DF__post_comm__publi__6B24EA82]
GO
ALTER TABLE [blog].[post_comment] DROP CONSTRAINT [DF__post_comm__publi__6A30C649]
GO
ALTER TABLE [blog].[post_comment] DROP CONSTRAINT [DF__post_comm__paren__693CA210]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [DF__post__publishedA__619B8048]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [DF__post__updatedAt__60A75C0F]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [DF__post__published__5FB337D6]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [DF__post__metaTitle__5EBF139D]
GO
ALTER TABLE [blog].[post] DROP CONSTRAINT [DF__post__parentId__5DCAEF64]
GO
ALTER TABLE [blog].[category] DROP CONSTRAINT [DF__category__metaTi__59063A47]
GO
ALTER TABLE [blog].[category] DROP CONSTRAINT [DF__category__parent__5812160E]
GO
/****** Object:  Table [trip].[state]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[trip].[state]') AND type in (N'U'))
DROP TABLE [trip].[state]
GO
/****** Object:  Table [trip].[district]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[trip].[district]') AND type in (N'U'))
DROP TABLE [trip].[district]
GO
/****** Object:  Table [trip].[city]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[trip].[city]') AND type in (N'U'))
DROP TABLE [trip].[city]
GO
/****** Object:  Table [journey].[location]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[location]') AND type in (N'U'))
DROP TABLE [journey].[location]
GO
/****** Object:  Table [journey].[help_contact_type]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[help_contact_type]') AND type in (N'U'))
DROP TABLE [journey].[help_contact_type]
GO
/****** Object:  Table [journey].[help_contact]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[help_contact]') AND type in (N'U'))
DROP TABLE [journey].[help_contact]
GO
/****** Object:  Table [journey].[faq]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[faq]') AND type in (N'U'))
DROP TABLE [journey].[faq]
GO
/****** Object:  Table [journey].[event_type]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[event_type]') AND type in (N'U'))
DROP TABLE [journey].[event_type]
GO
/****** Object:  Table [journey].[event]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[event]') AND type in (N'U'))
DROP TABLE [journey].[event]
GO
/****** Object:  Table [journey].[attraction]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[attraction]') AND type in (N'U'))
DROP TABLE [journey].[attraction]
GO
/****** Object:  Table [journey].[activity_type]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[activity_type]') AND type in (N'U'))
DROP TABLE [journey].[activity_type]
GO
/****** Object:  Table [journey].[activity]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[journey].[activity]') AND type in (N'U'))
DROP TABLE [journey].[activity]
GO
/****** Object:  Table [blog].[tag]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[tag]') AND type in (N'U'))
DROP TABLE [blog].[tag]
GO
/****** Object:  Table [blog].[post_tag]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[post_tag]') AND type in (N'U'))
DROP TABLE [blog].[post_tag]
GO
/****** Object:  Table [blog].[post_meta]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[post_meta]') AND type in (N'U'))
DROP TABLE [blog].[post_meta]
GO
/****** Object:  Table [blog].[post_comment]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[post_comment]') AND type in (N'U'))
DROP TABLE [blog].[post_comment]
GO
/****** Object:  Table [blog].[post_category]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[post_category]') AND type in (N'U'))
DROP TABLE [blog].[post_category]
GO
/****** Object:  Table [blog].[post]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[post]') AND type in (N'U'))
DROP TABLE [blog].[post]
GO
/****** Object:  Table [blog].[category]    Script Date: 10/8/2022 11:14:48 PM ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[blog].[category]') AND type in (N'U'))
DROP TABLE [blog].[category]
GO
/****** Object:  Schema [yari]    Script Date: 10/8/2022 11:14:48 PM ******/
DROP SCHEMA [yari]
GO
/****** Object:  Schema [trip]    Script Date: 10/8/2022 11:14:48 PM ******/
DROP SCHEMA [trip]
GO
/****** Object:  Schema [journey]    Script Date: 10/8/2022 11:14:48 PM ******/
DROP SCHEMA [journey]
GO
/****** Object:  Schema [blog]    Script Date: 10/8/2022 11:14:48 PM ******/
DROP SCHEMA [blog]
GO
/****** Object:  Schema [blog]    Script Date: 10/8/2022 11:14:48 PM ******/
CREATE SCHEMA [blog]
GO
/****** Object:  Schema [journey]    Script Date: 10/8/2022 11:14:48 PM ******/
CREATE SCHEMA [journey]
GO
/****** Object:  Schema [trip]    Script Date: 10/8/2022 11:14:48 PM ******/
CREATE SCHEMA [trip]
GO
/****** Object:  Schema [yari]    Script Date: 10/8/2022 11:14:48 PM ******/
CREATE SCHEMA [yari]
GO
/****** Object:  Table [blog].[category]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[category](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[parentId] [bigint] NULL,
	[title] [varchar](75) NOT NULL,
	[metaTitle] [varchar](100) NULL,
	[slug] [varchar](100) NOT NULL,
	[content] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [blog].[post]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[post](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[authorId] [bigint] NOT NULL,
	[parentId] [bigint] NULL,
	[title] [varchar](75) NOT NULL,
	[metaTitle] [varchar](100) NULL,
	[slug] [varchar](100) NOT NULL,
	[summary] [varchar](255) NULL,
	[published] [smallint] NOT NULL,
	[created_on] [datetime2](0) NOT NULL,
	[updated_on] [datetime2](0) NULL,
	[published_on] [datetime2](0) NULL,
	[html_content] [varchar](max) NULL,
	[location_id] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uq_slug] UNIQUE NONCLUSTERED 
(
	[slug] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [blog].[post_category]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[post_category](
	[postId] [bigint] NOT NULL,
	[categoryId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[postId] ASC,
	[categoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [blog].[post_comment]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[post_comment](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[postId] [bigint] NOT NULL,
	[parentId] [bigint] NULL,
	[title] [varchar](100) NOT NULL,
	[published] [smallint] NOT NULL,
	[createdAt] [datetime2](0) NOT NULL,
	[publishedAt] [datetime2](0) NULL,
	[content] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [blog].[post_meta]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[post_meta](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[postId] [bigint] NOT NULL,
	[key] [varchar](50) NOT NULL,
	[content] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [uq_post_meta] UNIQUE NONCLUSTERED 
(
	[postId] ASC,
	[key] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [blog].[post_tag]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[post_tag](
	[postId] [bigint] NOT NULL,
	[tagId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[postId] ASC,
	[tagId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [blog].[tag]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [blog].[tag](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[title] [varchar](75) NOT NULL,
	[metaTitle] [varchar](100) NULL,
	[slug] [varchar](100) NOT NULL,
	[content] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [journey].[activity]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[activity](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[attraction_id] [int] NULL,
	[location_id] [int] NOT NULL,
	[title] [varchar](75) NOT NULL,
	[sub_title] [varchar](150) NULL,
	[description] [varchar](1500) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[activity_type]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[activity_type](
	[id] [tinyint] IDENTITY(1,1) NOT NULL,
	[description] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[attraction]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[attraction](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[location_id] [int] NOT NULL,
	[title] [varchar](75) NOT NULL,
	[sub_title] [varchar](150) NULL,
	[description] [varchar](1500) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[event]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[event](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type_id] [tinyint] NOT NULL,
	[attraction_id] [int] NULL,
	[location_id] [int] NOT NULL,
	[title] [varchar](75) NOT NULL,
	[sub_title] [varchar](150) NULL,
	[description] [varchar](1500) NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[event_type]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[event_type](
	[id] [tinyint] IDENTITY(1,1) NOT NULL,
	[description] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[faq]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[faq](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type_id] [tinyint] NULL,
	[attraction_id] [int] NULL,
	[location_id] [int] NULL,
	[Question] [varchar](150) NULL,
	[answer] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[help_contact]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[help_contact](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[type_id] [tinyint] NULL,
	[attraction_id] [int] NULL,
	[location_id] [int] NULL,
	[title] [varchar](75) NULL,
	[sub_title] [varchar](150) NULL,
	[description] [varchar](500) NULL,
	[phone_number] [varchar](15) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[help_contact_type]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[help_contact_type](
	[id] [tinyint] IDENTITY(1,1) NOT NULL,
	[description] [varchar](500) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [journey].[location]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [journey].[location](
	[id] [bigint] IDENTITY(1,1) NOT NULL,
	[title] [varchar](75) NULL,
	[sub_title] [varchar](150) NULL,
	[description] [varchar](500) NULL,
	[city_place] [int] NULL
) ON [PRIMARY]
GO
/****** Object:  Table [trip].[city]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [trip].[city](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[name] [varchar](100) NOT NULL,
	[district_id] [int] NULL,
	[state_id] [int] NULL,
	[description] [varchar](max) NOT NULL,
	[status] [varchar](10) NULL,
	[pincode] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [trip].[district]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [trip].[district](
	[district_id] [int] IDENTITY(1,1) NOT NULL,
	[district_title] [varchar](100) NOT NULL,
	[state_id] [int] NULL,
	[district_description] [varchar](max) NOT NULL,
	[district_status] [varchar](10) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [trip].[state]    Script Date: 10/8/2022 11:14:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [trip].[state](
	[state_id] [int] IDENTITY(1,1) NOT NULL,
	[state_title] [varchar](100) NOT NULL,
	[state_description] [varchar](max) NOT NULL,
	[status] [varchar](10) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [blog].[category] ADD  DEFAULT (NULL) FOR [parentId]
GO
ALTER TABLE [blog].[category] ADD  DEFAULT (NULL) FOR [metaTitle]
GO
ALTER TABLE [blog].[post] ADD  DEFAULT (NULL) FOR [parentId]
GO
ALTER TABLE [blog].[post] ADD  DEFAULT (NULL) FOR [metaTitle]
GO
ALTER TABLE [blog].[post] ADD  DEFAULT ('0') FOR [published]
GO
ALTER TABLE [blog].[post] ADD  DEFAULT (NULL) FOR [updated_on]
GO
ALTER TABLE [blog].[post] ADD  DEFAULT (NULL) FOR [published_on]
GO
ALTER TABLE [blog].[post_comment] ADD  DEFAULT (NULL) FOR [parentId]
GO
ALTER TABLE [blog].[post_comment] ADD  DEFAULT ('0') FOR [published]
GO
ALTER TABLE [blog].[post_comment] ADD  DEFAULT (NULL) FOR [publishedAt]
GO
ALTER TABLE [blog].[tag] ADD  DEFAULT (NULL) FOR [metaTitle]
GO
ALTER TABLE [trip].[city] ADD  DEFAULT (NULL) FOR [district_id]
GO
ALTER TABLE [trip].[city] ADD  DEFAULT (NULL) FOR [state_id]
GO
ALTER TABLE [trip].[city] ADD  DEFAULT (NULL) FOR [status]
GO
ALTER TABLE [trip].[district] ADD  DEFAULT (NULL) FOR [state_id]
GO
ALTER TABLE [blog].[category]  WITH CHECK ADD  CONSTRAINT [fk_category_parent] FOREIGN KEY([parentId])
REFERENCES [blog].[category] ([id])
GO
ALTER TABLE [blog].[category] CHECK CONSTRAINT [fk_category_parent]
GO
ALTER TABLE [blog].[post]  WITH CHECK ADD  CONSTRAINT [fk_post_parent] FOREIGN KEY([parentId])
REFERENCES [blog].[post] ([id])
GO
ALTER TABLE [blog].[post] CHECK CONSTRAINT [fk_post_parent]
GO
ALTER TABLE [blog].[post_category]  WITH CHECK ADD  CONSTRAINT [fk_pc_category] FOREIGN KEY([categoryId])
REFERENCES [blog].[category] ([id])
GO
ALTER TABLE [blog].[post_category] CHECK CONSTRAINT [fk_pc_category]
GO
ALTER TABLE [blog].[post_category]  WITH CHECK ADD  CONSTRAINT [fk_pc_post] FOREIGN KEY([postId])
REFERENCES [blog].[post] ([id])
GO
ALTER TABLE [blog].[post_category] CHECK CONSTRAINT [fk_pc_post]
GO
ALTER TABLE [blog].[post_comment]  WITH CHECK ADD  CONSTRAINT [fk_comment_parent] FOREIGN KEY([parentId])
REFERENCES [blog].[post_comment] ([id])
GO
ALTER TABLE [blog].[post_comment] CHECK CONSTRAINT [fk_comment_parent]
GO
ALTER TABLE [blog].[post_comment]  WITH CHECK ADD  CONSTRAINT [fk_comment_post] FOREIGN KEY([postId])
REFERENCES [blog].[post] ([id])
GO
ALTER TABLE [blog].[post_comment] CHECK CONSTRAINT [fk_comment_post]
GO
ALTER TABLE [blog].[post_meta]  WITH CHECK ADD  CONSTRAINT [fk_meta_post] FOREIGN KEY([postId])
REFERENCES [blog].[post] ([id])
GO
ALTER TABLE [blog].[post_meta] CHECK CONSTRAINT [fk_meta_post]
GO
ALTER TABLE [blog].[post_tag]  WITH CHECK ADD  CONSTRAINT [fk_pt_post] FOREIGN KEY([postId])
REFERENCES [blog].[post] ([id])
GO
ALTER TABLE [blog].[post_tag] CHECK CONSTRAINT [fk_pt_post]
GO
ALTER TABLE [blog].[post_tag]  WITH CHECK ADD  CONSTRAINT [fk_pt_tag] FOREIGN KEY([tagId])
REFERENCES [blog].[tag] ([id])
GO
ALTER TABLE [blog].[post_tag] CHECK CONSTRAINT [fk_pt_tag]
GO
