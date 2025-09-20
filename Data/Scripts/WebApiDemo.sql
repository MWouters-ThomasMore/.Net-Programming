/*
--------------------------------------------------
	Voer "eerst" de commands uit via console:

	Add-Migration MIG1
	Update-Database
--------------------------------------------------
     Klanten, Producten zaten al in vorige hfstukken in dat sql-bestand.
     Voer daarna dit bestand pas uit:
*/

SET IDENTITY_INSERT [dbo].[Klant] ON
INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [AangemaaktDatum]) VALUES (1, N'Van Der Neffe', N'Leon', N'2022-10-01 00:00:00')
INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [AangemaaktDatum]) VALUES (2, N'Van De Kasseinen', N'Firmin', N'2022-10-02 00:00:00')
INSERT INTO [dbo].[Klant] ([Id], [Naam], [Voornaam], [AangemaaktDatum]) VALUES (3, N'Kiekeboe', N'Marcel', N'2022-10-03 00:00:00')
SET IDENTITY_INSERT [dbo].[Klant] OFF

SET IDENTITY_INSERT [dbo].[Product] ON
INSERT INTO [dbo].[Product] ([Id], [Naam], [Prijs], [Beschrijving]) VALUES (1, N'fiets', CAST(100.00 AS Decimal(18, 2)), N'Dit is een fiets')
INSERT INTO [dbo].[Product] ([Id], [Naam], [Prijs], [Beschrijving]) VALUES (2, N'koersfiets', CAST(200.00 AS Decimal(18, 2)), N'Dit is een mooie koersfiets')
INSERT INTO [dbo].[Product] ([Id], [Naam], [Prijs], [Beschrijving]) VALUES (3, N'auto', CAST(2000.00 AS Decimal(18, 2)), N'Dit is een auto')
SET IDENTITY_INSERT [dbo].[Product] OFF

SET IDENTITY_INSERT [dbo].[Bestelling] ON
INSERT INTO [dbo].[Bestelling] ([Id], [KlantId]) VALUES (1, 1)
INSERT INTO [dbo].[Bestelling] ([Id], [KlantId]) VALUES (2, 2)
INSERT INTO [dbo].[Bestelling] ([Id], [KlantId]) VALUES (3, 3)
INSERT INTO [dbo].[Bestelling] ([Id], [KlantId]) VALUES (4, 2)
INSERT INTO [dbo].[Bestelling] ([Id], [KlantId]) VALUES (5, 3)
SET IDENTITY_INSERT [dbo].[Bestelling] OFF

SET IDENTITY_INSERT [dbo].[OrderLijn] ON
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (1, 3, 1, 1)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (2, 7, 1, 2)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (3, 4, 2, 1)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (4, 1, 2, 2)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (5, 2, 3, 1)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (6, 3, 4, 1)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (7, 1, 4, 3)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (8, 2, 5, 1)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (9, 6, 5, 2)
INSERT INTO [dbo].[OrderLijn] ([Id], [Aantal], [BestellingId], [ProductId]) VALUES (10, 10, 5, 3)
SET IDENTITY_INSERT [dbo].[OrderLijn] OFF
