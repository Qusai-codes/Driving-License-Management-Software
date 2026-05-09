-- Complete Database Creation Script for SQL Server

-- ============================================
-- Tables (created in dependency order)
-- ============================================

-- Level 1: No dependencies
CREATE TABLE [dbo].[Countries] (
    [CountryID] INT IDENTITY(1,1) NOT NULL,
    [CountryName] NVARCHAR(50) NOT NULL,
    CONSTRAINT PK_Countries PRIMARY KEY (CountryID)
);

CREATE TABLE [dbo].[ApplicationTypes] (
    [ApplicationTypeID] INT IDENTITY(1,1) NOT NULL,
    [ApplicationTypeTitle] NVARCHAR(150) NOT NULL,
    [ApplicationFees] SMALLMONEY NOT NULL,
    CONSTRAINT PK_ApplicationTypes PRIMARY KEY (ApplicationTypeID)
);

CREATE TABLE [dbo].[LicenseClasses] (
    [LicenseClassID] INT IDENTITY(1,1) NOT NULL,
    [ClassName] NVARCHAR(50) NOT NULL,
    [ClassDescription] NVARCHAR(500) NOT NULL,
    [MinimumAllowedAge] TINYINT NOT NULL,
    [DefaultValidityLength] TINYINT NOT NULL,
    [ClassFees] SMALLMONEY NOT NULL,
    CONSTRAINT PK_LicenseClasses PRIMARY KEY (LicenseClassID)
);

CREATE TABLE [dbo].[TestTypes] (
    [TestTypeID] INT IDENTITY(1,1) NOT NULL,
    [TestTypeTitle] NVARCHAR(100) NOT NULL,
    [TestTypeDescription] NVARCHAR(500) NOT NULL,
    [TestTypeFees] SMALLMONEY NOT NULL,
    CONSTRAINT PK_TestTypes PRIMARY KEY (TestTypeID)
);

-- Level 2: Depends on Countries
CREATE TABLE [dbo].[People] (
    [PersonID] INT IDENTITY(1,1) NOT NULL,
    [NationalNo] NVARCHAR(20) NOT NULL,
    [FirstName] NVARCHAR(20) NOT NULL,
    [SecondName] NVARCHAR(20) NOT NULL,
    [ThirdName] NVARCHAR(20) NULL,
    [LastName] NVARCHAR(20) NOT NULL,
    [DateOfBirth] DATETIME NOT NULL,
    [Gendor] TINYINT NOT NULL,
    [Address] NVARCHAR(500) NOT NULL,
    [Phone] NVARCHAR(20) NOT NULL,
    [Email] NVARCHAR(50) NULL,
    [NationalityCountryID] INT NOT NULL,
    [ImagePath] NVARCHAR(250) NULL,
    CONSTRAINT PK_People PRIMARY KEY (PersonID),
    CONSTRAINT FK_People_Countries1 FOREIGN KEY (NationalityCountryID) REFERENCES [dbo].[Countries](CountryID)
);

-- Level 3: Depends on People
CREATE TABLE [dbo].[Users] (
    [UserID] INT IDENTITY(1,1) NOT NULL,
    [PersonID] INT NOT NULL,
    [UserName] NVARCHAR(20) NOT NULL,
    [IsActive] BIT NOT NULL,
    [PasswordHash] NVARCHAR(200) NOT NULL,
    [PasswordSalt] NVARCHAR(200) NOT NULL,
    CONSTRAINT PK_Users PRIMARY KEY (UserID),
    CONSTRAINT FK_Users_People FOREIGN KEY (PersonID) REFERENCES [dbo].[People](PersonID)
);

-- Level 4: Depends on People, ApplicationTypes, Users
CREATE TABLE [dbo].[Applications] (
    [ApplicationID] INT IDENTITY(1,1) NOT NULL,
    [ApplicantPersonID] INT NOT NULL,
    [ApplicationDate] DATETIME NOT NULL,
    [ApplicationTypeID] INT NOT NULL,
    [ApplicationStatus] TINYINT NOT NULL,
    [LastStatusDate] DATETIME NOT NULL,
    [PaidFees] SMALLMONEY NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    CONSTRAINT PK_Applications PRIMARY KEY (ApplicationID),
    CONSTRAINT FK_Applications_People FOREIGN KEY (ApplicantPersonID) REFERENCES [dbo].[People](PersonID),
    CONSTRAINT FK_Applications_ApplicationTypes FOREIGN KEY (ApplicationTypeID) REFERENCES [dbo].[ApplicationTypes](ApplicationTypeID),
    CONSTRAINT FK_Applications_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID)
);

-- Level 5: Depends on People, Users
CREATE TABLE [dbo].[Drivers] (
    [DriverID] INT IDENTITY(1,1) NOT NULL,
    [PersonID] INT NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    [CreatedDate] SMALLDATETIME NOT NULL,
    CONSTRAINT PK_Drivers PRIMARY KEY (DriverID),
    CONSTRAINT FK_Drivers_People FOREIGN KEY (PersonID) REFERENCES [dbo].[People](PersonID),
    CONSTRAINT FK_Drivers_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID)
);

-- Level 6: Depends on Applications, LicenseClasses
CREATE TABLE [dbo].[LocalDrivingLicenseApplications] (
    [LocalDrivingLicenseApplicationID] INT IDENTITY(1,1) NOT NULL,
    [ApplicationID] INT NOT NULL,
    [LicenseClassID] INT NOT NULL,
    CONSTRAINT PK_LocalDrivingLicenseApplications PRIMARY KEY (LocalDrivingLicenseApplicationID),
    CONSTRAINT FK_DrivingLicsenseApplications_Applications FOREIGN KEY (ApplicationID) REFERENCES [dbo].[Applications](ApplicationID),
    CONSTRAINT FK_DrivingLicsenseApplications_LicenseClasses FOREIGN KEY (LicenseClassID) REFERENCES [dbo].[LicenseClasses](LicenseClassID)
);

-- Level 7: Depends on Applications, Drivers, LicenseClasses, Users
CREATE TABLE [dbo].[Licenses] (
    [LicenseID] INT IDENTITY(1,1) NOT NULL,
    [ApplicationID] INT NOT NULL,
    [DriverID] INT NOT NULL,
    [LicenseClass] INT NOT NULL,
    [IssueDate] DATETIME NOT NULL,
    [ExpirationDate] DATETIME NOT NULL,
    [Notes] NVARCHAR(500) NULL,
    [PaidFees] SMALLMONEY NOT NULL,
    [IsActive] BIT NOT NULL,
    [IssueReason] TINYINT NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    CONSTRAINT PK_Licenses PRIMARY KEY (LicenseID),
    CONSTRAINT FK_Licenses_Applications FOREIGN KEY (ApplicationID) REFERENCES [dbo].[Applications](ApplicationID),
    CONSTRAINT FK_Licenses_Drivers FOREIGN KEY (DriverID) REFERENCES [dbo].[Drivers](DriverID),
    CONSTRAINT FK_Licenses_LicenseClasses FOREIGN KEY (LicenseClass) REFERENCES [dbo].[LicenseClasses](LicenseClassID),
    CONSTRAINT FK_Licenses_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID)
);

-- Level 8: Depends on TestTypes, LocalDrivingLicenseApplications, Users, Applications (through RetakeTestApplicationID)
CREATE TABLE [dbo].[TestAppointments] (
    [TestAppointmentID] INT IDENTITY(1,1) NOT NULL,
    [TestTypeID] INT NOT NULL,
    [LocalDrivingLicenseApplicationID] INT NOT NULL,
    [AppointmentDate] SMALLDATETIME NOT NULL,
    [PaidFees] SMALLMONEY NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    [IsLocked] BIT NOT NULL,
    [RetakeTestApplicationID] INT NULL,
    CONSTRAINT PK_TestAppointments PRIMARY KEY (TestAppointmentID),
    CONSTRAINT FK_TestAppointments_TestTypes FOREIGN KEY (TestTypeID) REFERENCES [dbo].[TestTypes](TestTypeID),
    CONSTRAINT FK_TestAppointments_LocalDrivingLicenseApplications FOREIGN KEY (LocalDrivingLicenseApplicationID) REFERENCES [dbo].[LocalDrivingLicenseApplications](LocalDrivingLicenseApplicationID),
    CONSTRAINT FK_TestAppointments_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID),
    CONSTRAINT FK_TestAppointments_Applications FOREIGN KEY (RetakeTestApplicationID) REFERENCES [dbo].[Applications](ApplicationID)
);

-- Level 9: Depends on TestAppointments, Users
CREATE TABLE [dbo].[Tests] (
    [TestID] INT IDENTITY(1,1) NOT NULL,
    [TestAppointmentID] INT NOT NULL,
    [TestResult] BIT NOT NULL,
    [Notes] NVARCHAR(500) NULL,
    [CreatedByUserID] INT NOT NULL,
    CONSTRAINT PK_Tests PRIMARY KEY (TestID),
    CONSTRAINT FK_Tests_TestAppointments FOREIGN KEY (TestAppointmentID) REFERENCES [dbo].[TestAppointments](TestAppointmentID),
    CONSTRAINT FK_Tests_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID)
);

-- Level 10: Depends on Licenses, Users, Applications
CREATE TABLE [dbo].[DetainedLicenses] (
    [DetainID] INT IDENTITY(1,1) NOT NULL,
    [LicenseID] INT NOT NULL,
    [DetainDate] SMALLDATETIME NOT NULL,
    [FineFees] SMALLMONEY NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    [IsReleased] BIT NOT NULL,
    [ReleaseDate] SMALLDATETIME NULL,
    [ReleasedByUserID] INT NULL,
    [ReleaseApplicationID] INT NULL,
    CONSTRAINT PK_DetainedLicenses PRIMARY KEY (DetainID),
    CONSTRAINT FK_DetainedLicenses_Licenses FOREIGN KEY (LicenseID) REFERENCES [dbo].[Licenses](LicenseID),
    CONSTRAINT FK_DetainedLicenses_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID),
    CONSTRAINT FK_DetainedLicenses_Users1 FOREIGN KEY (ReleasedByUserID) REFERENCES [dbo].[Users](UserID),
    CONSTRAINT FK_DetainedLicenses_Applications FOREIGN KEY (ReleaseApplicationID) REFERENCES [dbo].[Applications](ApplicationID)
);

-- Level 11: Depends on Applications, Drivers, Licenses, Users
CREATE TABLE [dbo].[InternationalLicenses] (
    [InternationalLicenseID] INT IDENTITY(1,1) NOT NULL,
    [ApplicationID] INT NOT NULL,
    [DriverID] INT NOT NULL,
    [IssuedUsingLocalLicenseID] INT NOT NULL,
    [IssueDate] SMALLDATETIME NOT NULL,
    [ExpirationDate] SMALLDATETIME NOT NULL,
    [IsActive] BIT NOT NULL,
    [CreatedByUserID] INT NOT NULL,
    CONSTRAINT PK_InternationalLicenses PRIMARY KEY (InternationalLicenseID),
    CONSTRAINT FK_InternationalLicenses_Applications FOREIGN KEY (ApplicationID) REFERENCES [dbo].[Applications](ApplicationID),
    CONSTRAINT FK_InternationalLicenses_Drivers FOREIGN KEY (DriverID) REFERENCES [dbo].[Drivers](DriverID),
    CONSTRAINT FK_InternationalLicenses_Licenses FOREIGN KEY (IssuedUsingLocalLicenseID) REFERENCES [dbo].[Licenses](LicenseID),
    CONSTRAINT FK_InternationalLicenses_Users FOREIGN KEY (CreatedByUserID) REFERENCES [dbo].[Users](UserID)
);