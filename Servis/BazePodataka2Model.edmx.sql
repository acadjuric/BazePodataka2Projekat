
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 05/30/2021 13:13:39
-- Generated from EDMX file: C:\Users\Asus\Desktop\ProjekatBaze\ProjekatBazePodataka2\Servis\BazePodataka2Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [ProjekatBazePodataka2];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_KorisnikPrima]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrimljenaObavestenja] DROP CONSTRAINT [FK_KorisnikPrima];
GO
IF OBJECT_ID(N'[dbo].[FK_ObavestenjePrima]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PrimljenaObavestenja] DROP CONSTRAINT [FK_ObavestenjePrima];
GO
IF OBJECT_ID(N'[dbo].[FK_ObavestenjeKorisnik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Obavestenja] DROP CONSTRAINT [FK_ObavestenjeKorisnik];
GO
IF OBJECT_ID(N'[dbo].[FK_NastavnikKurs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Kursevi] DROP CONSTRAINT [FK_NastavnikKurs];
GO
IF OBJECT_ID(N'[dbo].[FK_UcenikPrijavljen]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Prijavljeni] DROP CONSTRAINT [FK_UcenikPrijavljen];
GO
IF OBJECT_ID(N'[dbo].[FK_PrijavljenKurs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Prijavljeni] DROP CONSTRAINT [FK_PrijavljenKurs];
GO
IF OBJECT_ID(N'[dbo].[FK_TestKurs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Testovi] DROP CONSTRAINT [FK_TestKurs];
GO
IF OBJECT_ID(N'[dbo].[FK_KursSadrzi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sadrzis] DROP CONSTRAINT [FK_KursSadrzi];
GO
IF OBJECT_ID(N'[dbo].[FK_NastavnaTemaSadrzi]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sadrzis] DROP CONSTRAINT [FK_NastavnaTemaSadrzi];
GO
IF OBJECT_ID(N'[dbo].[FK_TestPolaze]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Polazes] DROP CONSTRAINT [FK_TestPolaze];
GO
IF OBJECT_ID(N'[dbo].[FK_PrijavljenPolaze]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Polazes] DROP CONSTRAINT [FK_PrijavljenPolaze];
GO
IF OBJECT_ID(N'[dbo].[FK_TestSastoji]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sastojis] DROP CONSTRAINT [FK_TestSastoji];
GO
IF OBJECT_ID(N'[dbo].[FK_PitanjeSastoji]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sastojis] DROP CONSTRAINT [FK_PitanjeSastoji];
GO
IF OBJECT_ID(N'[dbo].[FK_PitanjePoseduje]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posedujes] DROP CONSTRAINT [FK_PitanjePoseduje];
GO
IF OBJECT_ID(N'[dbo].[FK_OdgovorPoseduje]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Posedujes] DROP CONSTRAINT [FK_OdgovorPoseduje];
GO
IF OBJECT_ID(N'[dbo].[FK_Nastavnik_inherits_Korisnik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Korisnici_Nastavnik] DROP CONSTRAINT [FK_Nastavnik_inherits_Korisnik];
GO
IF OBJECT_ID(N'[dbo].[FK_Ucenik_inherits_Korisnik]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Korisnici_Ucenik] DROP CONSTRAINT [FK_Ucenik_inherits_Korisnik];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Obavestenja]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Obavestenja];
GO
IF OBJECT_ID(N'[dbo].[Korisnici]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Korisnici];
GO
IF OBJECT_ID(N'[dbo].[PrimljenaObavestenja]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrimljenaObavestenja];
GO
IF OBJECT_ID(N'[dbo].[Kursevi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Kursevi];
GO
IF OBJECT_ID(N'[dbo].[NastavneTeme]', 'U') IS NOT NULL
    DROP TABLE [dbo].[NastavneTeme];
GO
IF OBJECT_ID(N'[dbo].[Prijavljeni]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Prijavljeni];
GO
IF OBJECT_ID(N'[dbo].[Testovi]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Testovi];
GO
IF OBJECT_ID(N'[dbo].[Pitanja]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Pitanja];
GO
IF OBJECT_ID(N'[dbo].[Odgovori]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Odgovori];
GO
IF OBJECT_ID(N'[dbo].[Sadrzis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sadrzis];
GO
IF OBJECT_ID(N'[dbo].[Polazes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Polazes];
GO
IF OBJECT_ID(N'[dbo].[Sastojis]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sastojis];
GO
IF OBJECT_ID(N'[dbo].[Posedujes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posedujes];
GO
IF OBJECT_ID(N'[dbo].[Korisnici_Nastavnik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Korisnici_Nastavnik];
GO
IF OBJECT_ID(N'[dbo].[Korisnici_Ucenik]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Korisnici_Ucenik];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Obavestenja'
CREATE TABLE [dbo].[Obavestenja] (
    [IdObavestenja] int IDENTITY(1,1) NOT NULL,
    [Naslov] nvarchar(max)  NOT NULL,
    [Tekst] nvarchar(max)  NOT NULL,
    [DatumSlanja] datetime  NOT NULL,
    [KorisnikIdKorisnika] int  NOT NULL
);
GO

-- Creating table 'Korisnici'
CREATE TABLE [dbo].[Korisnici] (
    [IdKorisnika] int  NOT NULL,
    [EmailAdresa] nvarchar(max)  NOT NULL,
    [Ime] nvarchar(max)  NOT NULL,
    [Prezime] nvarchar(max)  NOT NULL,
    [Sifra] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'PrimljenaObavestenja'
CREATE TABLE [dbo].[PrimljenaObavestenja] (
    [ObavestenjeIdObavestenja] int  NOT NULL,
    [KorisnikIdKorisnika] int  NOT NULL,
    [DatumCitanja] datetime  NOT NULL,
    [Obrisano] bit  NOT NULL
);
GO

-- Creating table 'Kursevi'
CREATE TABLE [dbo].[Kursevi] (
    [IdKursa] int IDENTITY(1,1) NOT NULL,
    [Naziv] nvarchar(max)  NOT NULL,
    [Poeni] int  NOT NULL,
    [NastavnikIdKorisnika] int  NOT NULL
);
GO

-- Creating table 'NastavneTeme'
CREATE TABLE [dbo].[NastavneTeme] (
    [IdTeme] int IDENTITY(1,1) NOT NULL,
    [Naziv] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Prijavljeni'
CREATE TABLE [dbo].[Prijavljeni] (
    [UcenikIdKorisnika] int  NOT NULL,
    [KursIdKursa] int  NOT NULL
);
GO

-- Creating table 'Testovi'
CREATE TABLE [dbo].[Testovi] (
    [IdTesta] int IDENTITY(1,1) NOT NULL,
    [Naziv] nvarchar(max)  NOT NULL,
    [Bodovi] int  NOT NULL,
    [Datum] datetime  NOT NULL,
    [KursIdKursa] int  NOT NULL
);
GO

-- Creating table 'Pitanja'
CREATE TABLE [dbo].[Pitanja] (
    [IdPitanja] int IDENTITY(1,1) NOT NULL,
    [Bodovi] int  NOT NULL,
    [Nivo] int  NOT NULL,
    [Tekst] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Odgovori'
CREATE TABLE [dbo].[Odgovori] (
    [IdOdgovora] int IDENTITY(1,1) NOT NULL,
    [Tekst] nvarchar(max)  NOT NULL,
    [Tacan] bit  NOT NULL
);
GO

-- Creating table 'Sadrzis'
CREATE TABLE [dbo].[Sadrzis] (
    [KursIdKursa] int  NOT NULL,
    [NastavnaTemaIdTeme] int  NOT NULL
);
GO

-- Creating table 'Polazes'
CREATE TABLE [dbo].[Polazes] (
    [TestIdTesta] int  NOT NULL,
    [PrijavljenUcenikIdKorisnika] int  NOT NULL,
    [PrijavljenKursIdKursa] int  NOT NULL
);
GO

-- Creating table 'Sastojis'
CREATE TABLE [dbo].[Sastojis] (
    [TestIdTesta] int  NOT NULL,
    [PitanjeIdPitanja] int  NOT NULL
);
GO

-- Creating table 'Posedujes'
CREATE TABLE [dbo].[Posedujes] (
    [PitanjeIdPitanja] int  NOT NULL,
    [OdgovorIdOdgovora] int  NOT NULL
);
GO

-- Creating table 'Korisnici_Nastavnik'
CREATE TABLE [dbo].[Korisnici_Nastavnik] (
    [Zvanje] nvarchar(max)  NOT NULL,
    [IdKorisnika] int  NOT NULL
);
GO

-- Creating table 'Korisnici_Ucenik'
CREATE TABLE [dbo].[Korisnici_Ucenik] (
    [Razred] nvarchar(max)  NOT NULL,
    [IdKorisnika] int  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [IdObavestenja] in table 'Obavestenja'
ALTER TABLE [dbo].[Obavestenja]
ADD CONSTRAINT [PK_Obavestenja]
    PRIMARY KEY CLUSTERED ([IdObavestenja] ASC);
GO

-- Creating primary key on [IdKorisnika] in table 'Korisnici'
ALTER TABLE [dbo].[Korisnici]
ADD CONSTRAINT [PK_Korisnici]
    PRIMARY KEY CLUSTERED ([IdKorisnika] ASC);
GO

-- Creating primary key on [KorisnikIdKorisnika], [ObavestenjeIdObavestenja] in table 'PrimljenaObavestenja'
ALTER TABLE [dbo].[PrimljenaObavestenja]
ADD CONSTRAINT [PK_PrimljenaObavestenja]
    PRIMARY KEY CLUSTERED ([KorisnikIdKorisnika], [ObavestenjeIdObavestenja] ASC);
GO

-- Creating primary key on [IdKursa] in table 'Kursevi'
ALTER TABLE [dbo].[Kursevi]
ADD CONSTRAINT [PK_Kursevi]
    PRIMARY KEY CLUSTERED ([IdKursa] ASC);
GO

-- Creating primary key on [IdTeme] in table 'NastavneTeme'
ALTER TABLE [dbo].[NastavneTeme]
ADD CONSTRAINT [PK_NastavneTeme]
    PRIMARY KEY CLUSTERED ([IdTeme] ASC);
GO

-- Creating primary key on [UcenikIdKorisnika], [KursIdKursa] in table 'Prijavljeni'
ALTER TABLE [dbo].[Prijavljeni]
ADD CONSTRAINT [PK_Prijavljeni]
    PRIMARY KEY CLUSTERED ([UcenikIdKorisnika], [KursIdKursa] ASC);
GO

-- Creating primary key on [IdTesta] in table 'Testovi'
ALTER TABLE [dbo].[Testovi]
ADD CONSTRAINT [PK_Testovi]
    PRIMARY KEY CLUSTERED ([IdTesta] ASC);
GO

-- Creating primary key on [IdPitanja] in table 'Pitanja'
ALTER TABLE [dbo].[Pitanja]
ADD CONSTRAINT [PK_Pitanja]
    PRIMARY KEY CLUSTERED ([IdPitanja] ASC);
GO

-- Creating primary key on [IdOdgovora] in table 'Odgovori'
ALTER TABLE [dbo].[Odgovori]
ADD CONSTRAINT [PK_Odgovori]
    PRIMARY KEY CLUSTERED ([IdOdgovora] ASC);
GO

-- Creating primary key on [KursIdKursa], [NastavnaTemaIdTeme] in table 'Sadrzis'
ALTER TABLE [dbo].[Sadrzis]
ADD CONSTRAINT [PK_Sadrzis]
    PRIMARY KEY CLUSTERED ([KursIdKursa], [NastavnaTemaIdTeme] ASC);
GO

-- Creating primary key on [TestIdTesta], [PrijavljenUcenikIdKorisnika], [PrijavljenKursIdKursa] in table 'Polazes'
ALTER TABLE [dbo].[Polazes]
ADD CONSTRAINT [PK_Polazes]
    PRIMARY KEY CLUSTERED ([TestIdTesta], [PrijavljenUcenikIdKorisnika], [PrijavljenKursIdKursa] ASC);
GO

-- Creating primary key on [TestIdTesta], [PitanjeIdPitanja] in table 'Sastojis'
ALTER TABLE [dbo].[Sastojis]
ADD CONSTRAINT [PK_Sastojis]
    PRIMARY KEY CLUSTERED ([TestIdTesta], [PitanjeIdPitanja] ASC);
GO

-- Creating primary key on [PitanjeIdPitanja], [OdgovorIdOdgovora] in table 'Posedujes'
ALTER TABLE [dbo].[Posedujes]
ADD CONSTRAINT [PK_Posedujes]
    PRIMARY KEY CLUSTERED ([PitanjeIdPitanja], [OdgovorIdOdgovora] ASC);
GO

-- Creating primary key on [IdKorisnika] in table 'Korisnici_Nastavnik'
ALTER TABLE [dbo].[Korisnici_Nastavnik]
ADD CONSTRAINT [PK_Korisnici_Nastavnik]
    PRIMARY KEY CLUSTERED ([IdKorisnika] ASC);
GO

-- Creating primary key on [IdKorisnika] in table 'Korisnici_Ucenik'
ALTER TABLE [dbo].[Korisnici_Ucenik]
ADD CONSTRAINT [PK_Korisnici_Ucenik]
    PRIMARY KEY CLUSTERED ([IdKorisnika] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [KorisnikIdKorisnika] in table 'PrimljenaObavestenja'
ALTER TABLE [dbo].[PrimljenaObavestenja]
ADD CONSTRAINT [FK_KorisnikPrima]
    FOREIGN KEY ([KorisnikIdKorisnika])
    REFERENCES [dbo].[Korisnici]
        ([IdKorisnika])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [ObavestenjeIdObavestenja] in table 'PrimljenaObavestenja'
ALTER TABLE [dbo].[PrimljenaObavestenja]
ADD CONSTRAINT [FK_ObavestenjePrima]
    FOREIGN KEY ([ObavestenjeIdObavestenja])
    REFERENCES [dbo].[Obavestenja]
        ([IdObavestenja])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ObavestenjePrima'
CREATE INDEX [IX_FK_ObavestenjePrima]
ON [dbo].[PrimljenaObavestenja]
    ([ObavestenjeIdObavestenja]);
GO

-- Creating foreign key on [KorisnikIdKorisnika] in table 'Obavestenja'
ALTER TABLE [dbo].[Obavestenja]
ADD CONSTRAINT [FK_ObavestenjeKorisnik]
    FOREIGN KEY ([KorisnikIdKorisnika])
    REFERENCES [dbo].[Korisnici]
        ([IdKorisnika])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ObavestenjeKorisnik'
CREATE INDEX [IX_FK_ObavestenjeKorisnik]
ON [dbo].[Obavestenja]
    ([KorisnikIdKorisnika]);
GO

-- Creating foreign key on [NastavnikIdKorisnika] in table 'Kursevi'
ALTER TABLE [dbo].[Kursevi]
ADD CONSTRAINT [FK_NastavnikKurs]
    FOREIGN KEY ([NastavnikIdKorisnika])
    REFERENCES [dbo].[Korisnici_Nastavnik]
        ([IdKorisnika])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NastavnikKurs'
CREATE INDEX [IX_FK_NastavnikKurs]
ON [dbo].[Kursevi]
    ([NastavnikIdKorisnika]);
GO

-- Creating foreign key on [UcenikIdKorisnika] in table 'Prijavljeni'
ALTER TABLE [dbo].[Prijavljeni]
ADD CONSTRAINT [FK_UcenikPrijavljen]
    FOREIGN KEY ([UcenikIdKorisnika])
    REFERENCES [dbo].[Korisnici_Ucenik]
        ([IdKorisnika])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [KursIdKursa] in table 'Prijavljeni'
ALTER TABLE [dbo].[Prijavljeni]
ADD CONSTRAINT [FK_PrijavljenKurs]
    FOREIGN KEY ([KursIdKursa])
    REFERENCES [dbo].[Kursevi]
        ([IdKursa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrijavljenKurs'
CREATE INDEX [IX_FK_PrijavljenKurs]
ON [dbo].[Prijavljeni]
    ([KursIdKursa]);
GO

-- Creating foreign key on [KursIdKursa] in table 'Testovi'
ALTER TABLE [dbo].[Testovi]
ADD CONSTRAINT [FK_TestKurs]
    FOREIGN KEY ([KursIdKursa])
    REFERENCES [dbo].[Kursevi]
        ([IdKursa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TestKurs'
CREATE INDEX [IX_FK_TestKurs]
ON [dbo].[Testovi]
    ([KursIdKursa]);
GO

-- Creating foreign key on [KursIdKursa] in table 'Sadrzis'
ALTER TABLE [dbo].[Sadrzis]
ADD CONSTRAINT [FK_KursSadrzi]
    FOREIGN KEY ([KursIdKursa])
    REFERENCES [dbo].[Kursevi]
        ([IdKursa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [NastavnaTemaIdTeme] in table 'Sadrzis'
ALTER TABLE [dbo].[Sadrzis]
ADD CONSTRAINT [FK_NastavnaTemaSadrzi]
    FOREIGN KEY ([NastavnaTemaIdTeme])
    REFERENCES [dbo].[NastavneTeme]
        ([IdTeme])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_NastavnaTemaSadrzi'
CREATE INDEX [IX_FK_NastavnaTemaSadrzi]
ON [dbo].[Sadrzis]
    ([NastavnaTemaIdTeme]);
GO

-- Creating foreign key on [TestIdTesta] in table 'Polazes'
ALTER TABLE [dbo].[Polazes]
ADD CONSTRAINT [FK_TestPolaze]
    FOREIGN KEY ([TestIdTesta])
    REFERENCES [dbo].[Testovi]
        ([IdTesta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PrijavljenUcenikIdKorisnika], [PrijavljenKursIdKursa] in table 'Polazes'
ALTER TABLE [dbo].[Polazes]
ADD CONSTRAINT [FK_PrijavljenPolaze]
    FOREIGN KEY ([PrijavljenUcenikIdKorisnika], [PrijavljenKursIdKursa])
    REFERENCES [dbo].[Prijavljeni]
        ([UcenikIdKorisnika], [KursIdKursa])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PrijavljenPolaze'
CREATE INDEX [IX_FK_PrijavljenPolaze]
ON [dbo].[Polazes]
    ([PrijavljenUcenikIdKorisnika], [PrijavljenKursIdKursa]);
GO

-- Creating foreign key on [TestIdTesta] in table 'Sastojis'
ALTER TABLE [dbo].[Sastojis]
ADD CONSTRAINT [FK_TestSastoji]
    FOREIGN KEY ([TestIdTesta])
    REFERENCES [dbo].[Testovi]
        ([IdTesta])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PitanjeIdPitanja] in table 'Sastojis'
ALTER TABLE [dbo].[Sastojis]
ADD CONSTRAINT [FK_PitanjeSastoji]
    FOREIGN KEY ([PitanjeIdPitanja])
    REFERENCES [dbo].[Pitanja]
        ([IdPitanja])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PitanjeSastoji'
CREATE INDEX [IX_FK_PitanjeSastoji]
ON [dbo].[Sastojis]
    ([PitanjeIdPitanja]);
GO

-- Creating foreign key on [PitanjeIdPitanja] in table 'Posedujes'
ALTER TABLE [dbo].[Posedujes]
ADD CONSTRAINT [FK_PitanjePoseduje]
    FOREIGN KEY ([PitanjeIdPitanja])
    REFERENCES [dbo].[Pitanja]
        ([IdPitanja])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [OdgovorIdOdgovora] in table 'Posedujes'
ALTER TABLE [dbo].[Posedujes]
ADD CONSTRAINT [FK_OdgovorPoseduje]
    FOREIGN KEY ([OdgovorIdOdgovora])
    REFERENCES [dbo].[Odgovori]
        ([IdOdgovora])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OdgovorPoseduje'
CREATE INDEX [IX_FK_OdgovorPoseduje]
ON [dbo].[Posedujes]
    ([OdgovorIdOdgovora]);
GO

-- Creating foreign key on [IdKorisnika] in table 'Korisnici_Nastavnik'
ALTER TABLE [dbo].[Korisnici_Nastavnik]
ADD CONSTRAINT [FK_Nastavnik_inherits_Korisnik]
    FOREIGN KEY ([IdKorisnika])
    REFERENCES [dbo].[Korisnici]
        ([IdKorisnika])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [IdKorisnika] in table 'Korisnici_Ucenik'
ALTER TABLE [dbo].[Korisnici_Ucenik]
ADD CONSTRAINT [FK_Ucenik_inherits_Korisnik]
    FOREIGN KEY ([IdKorisnika])
    REFERENCES [dbo].[Korisnici]
        ([IdKorisnika])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------