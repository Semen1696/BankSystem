# Bank System
Create local db and initial catalog Bank.
Create the following tables:
```sh
CREATE TABLE [dbo].[Accounts] (
    [Id]          INT             IDENTITY (1, 1) NOT NULL,
    [Number]      NVARCHAR (50)   NULL,
    [Amount]      DECIMAL (18, 2) NULL,
    [Createdate]  DATETIME        NULL,
    [AccountType] INT             NULL,
    [ClientId]    INT             NULL
);
```

```sh
CREATE TABLE [dbo].[Clients] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [Name]         NVARCHAR (50) NULL,
    [Surname]      NVARCHAR (50) NULL,
    [Age]          INT           NULL,
    [Phone]        NVARCHAR (10) NULL,
    [ClientTypeId] INT           NULL
);
```

```sh
CREATE TABLE [dbo].[Logs] (
    [Id]         INT           IDENTITY (1, 1) NOT NULL,
    [Text]       NVARCHAR (50) NULL,
    [CreateDate] DATETIME      NULL
);
```
