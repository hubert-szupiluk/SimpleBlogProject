
	
	
	CREATE TABLE [dbo].[Posts] (
    [PostId]      INT             IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (200)  NOT NULL,
    [Content]     NVARCHAR (1000) NOT NULL,
    [User_UserId] INT             NOT NULL,
    CONSTRAINT [PK_dbo.Posts] PRIMARY KEY CLUSTERED ([PostId] ASC),
    CONSTRAINT [FK_dbo.Posts_dbo.Users_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[Posts]([User_UserId] ASC);

	
	CREATE TABLE [dbo].[Tags] (
    [TagId]       INT            IDENTITY (1, 1) NOT NULL,
    [Tag_Name]    NVARCHAR (MAX) NULL,
    [Post_PostId] INT            NULL,
    [User_UserId] INT            NULL,
    CONSTRAINT [PK_dbo.Tags] PRIMARY KEY CLUSTERED ([TagId] ASC),
    CONSTRAINT [FK_dbo.Tags_dbo.Posts_Post_PostId] FOREIGN KEY ([Post_PostId]) REFERENCES [dbo].[Posts] ([PostId]),
    CONSTRAINT [FK_dbo.Tags_dbo.Users_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Post_PostId]
    ON [dbo].[Tags]([Post_PostId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[Tags]([User_UserId] ASC);



CREATE TABLE [dbo].[Comments] (
    [CommentId]   INT            IDENTITY (1, 1) NOT NULL,
    [Title]       NVARCHAR (MAX) NULL,
    [Content]     NVARCHAR (MAX) NOT NULL,
    [Post_PostId] INT            NULL,
    [User_UserId] INT            NULL,
    CONSTRAINT [PK_dbo.Comments] PRIMARY KEY CLUSTERED ([CommentId] ASC),
    CONSTRAINT [FK_dbo.Comments_dbo.Posts_Post_PostId] FOREIGN KEY ([Post_PostId]) REFERENCES [dbo].[Posts] ([PostId]),
    CONSTRAINT [FK_dbo.Comments_dbo.Users_User_UserId] FOREIGN KEY ([User_UserId]) REFERENCES [dbo].[Users] ([UserId])
);


GO
CREATE NONCLUSTERED INDEX [IX_Post_PostId]
    ON [dbo].[Comments]([Post_PostId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_User_UserId]
    ON [dbo].[Comments]([User_UserId] ASC);

