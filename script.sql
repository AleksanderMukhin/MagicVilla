IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [Villas] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Details] nvarchar(max) NOT NULL,
    [Rate] float NOT NULL,
    [Sqft] int NOT NULL,
    [Occupancy] int NOT NULL,
    [ImageUrl] nvarchar(max) NOT NULL,
    [Amenity] nvarchar(max) NOT NULL,
    [CreateDate] datetime2 NOT NULL,
    [UpdateDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Villas] PRIMARY KEY ([Id])
);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240503100557_Initial', N'7.0.0-preview.5.22302.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amenity', N'CreateDate', N'Details', N'ImageUrl', N'Name', N'Occupancy', N'Rate', N'Sqft', N'UpdateDate') AND [object_id] = OBJECT_ID(N'[Villas]'))
    SET IDENTITY_INSERT [Villas] ON;
INSERT INTO [Villas] ([Id], [Amenity], [CreateDate], [Details], [ImageUrl], [Name], [Occupancy], [Rate], [Sqft], [UpdateDate])
VALUES (1, N'', '0001-01-01T00:00:00.0000000', N'Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.', N'https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa3.jpg', N'Royal Villa', 4, 200.0E0, 550, '0001-01-01T00:00:00.0000000'),
(2, N'', '0001-01-01T00:00:00.0000000', N'Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.', N'https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa1.jpg', N'Premium Pool Villa', 4, 300.0E0, 550, '0001-01-01T00:00:00.0000000'),
(3, N'', '0001-01-01T00:00:00.0000000', N'Fusce 11 tincidunt maximus leo, sed scelerisque massa auctor sit amet. Donec ex mauris, hendrerit quis nibh ac, efficitur fringilla enim.', N'https://dotnetmasteryimages.blob.core.windows.net/bluevillaimages/villa4.jpg', N'Luxury Pool Villa', 4, 400.0E0, 750, '0001-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Amenity', N'CreateDate', N'Details', N'ImageUrl', N'Name', N'Occupancy', N'Rate', N'Sqft', N'UpdateDate') AND [object_id] = OBJECT_ID(N'[Villas]'))
    SET IDENTITY_INSERT [Villas] OFF;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240504092601_SeedVillaTable', N'7.0.0-preview.5.22302.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

UPDATE [Villas] SET [CreateDate] = '2024-05-04T11:31:44.8850250+02:00'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Villas] SET [CreateDate] = '2024-05-04T11:31:44.8850320+02:00'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Villas] SET [CreateDate] = '2024-05-04T11:31:44.8850330+02:00'
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240504093144_UpdateSeedVilla', N'7.0.0-preview.5.22302.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [VillaNombers] (
    [VillaNo] int NOT NULL,
    [SpecialDetails] nvarchar(max) NOT NULL,
    [CreatedDate] datetime2 NOT NULL,
    [UpdatedDate] datetime2 NOT NULL,
    CONSTRAINT [PK_VillaNombers] PRIMARY KEY ([VillaNo])
);
GO

IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'VillaNo', N'CreatedDate', N'SpecialDetails', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[VillaNombers]'))
    SET IDENTITY_INSERT [VillaNombers] ON;
INSERT INTO [VillaNombers] ([VillaNo], [CreatedDate], [SpecialDetails], [UpdatedDate])
VALUES (1, '2024-05-07T12:17:23.4896130+02:00', N'Special details for Villa 1', '0001-01-01T00:00:00.0000000'),
(2, '2024-05-07T12:17:23.4896130+02:00', N'Special details for Villa 2', '0001-01-01T00:00:00.0000000');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'VillaNo', N'CreatedDate', N'SpecialDetails', N'UpdatedDate') AND [object_id] = OBJECT_ID(N'[VillaNombers]'))
    SET IDENTITY_INSERT [VillaNombers] OFF;
GO

UPDATE [Villas] SET [CreateDate] = '2024-05-07T12:17:23.4896000+02:00'
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

UPDATE [Villas] SET [CreateDate] = '2024-05-07T12:17:23.4896050+02:00'
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

UPDATE [Villas] SET [CreateDate] = '2024-05-07T12:17:23.4896050+02:00'
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240507101723_VillaNombers', N'7.0.0-preview.5.22302.2');
GO

COMMIT;
GO

BEGIN TRANSACTION;
GO

DELETE FROM [VillaNombers]
WHERE [VillaNo] = 1;
SELECT @@ROWCOUNT;

GO

DELETE FROM [VillaNombers]
WHERE [VillaNo] = 2;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Villas]
WHERE [Id] = 1;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Villas]
WHERE [Id] = 2;
SELECT @@ROWCOUNT;

GO

DELETE FROM [Villas]
WHERE [Id] = 3;
SELECT @@ROWCOUNT;

GO

ALTER TABLE [VillaNombers] ADD [VillaID] int NOT NULL DEFAULT 0;
GO

CREATE INDEX [IX_VillaNombers_VillaID] ON [VillaNombers] ([VillaID]);
GO

ALTER TABLE [VillaNombers] ADD CONSTRAINT [FK_VillaNombers_Villas_VillaID] FOREIGN KEY ([VillaID]) REFERENCES [Villas] ([Id]) ON DELETE CASCADE;
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20240507172813_AddForiegn', N'7.0.0-preview.5.22302.2');
GO

COMMIT;
GO

