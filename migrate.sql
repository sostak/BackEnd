-- Create a new database for the migration
CREATE DATABASE IF NOT EXISTS Bakalauras_New;

-- Switch to the new database
USE Bakalauras_New;

-- Create tables with the new schema
CREATE TABLE AspNetUsers (
    Id varchar(255) NOT NULL,
    FirstName varchar(255) NOT NULL,
    LastName varchar(255) NOT NULL,
    PhoneNumber varchar(255),
    RefreshToken varchar(255),
    RefreshTokenExpiryTime datetime(6),
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Customers (
    Id varchar(255) NOT NULL,
    UserId varchar(255) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY IX_Customers_UserId (UserId),
    CONSTRAINT FK_Customers_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Mechanics (
    Id varchar(255) NOT NULL,
    UserId varchar(255) NOT NULL,
    PRIMARY KEY (Id),
    UNIQUE KEY IX_Mechanics_UserId (UserId),
    CONSTRAINT FK_Mechanics_AspNetUsers_UserId FOREIGN KEY (UserId) REFERENCES AspNetUsers (Id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Vehicles (
    Id varchar(255) NOT NULL,
    Make varchar(255) NOT NULL,
    Model varchar(255) NOT NULL,
    Year int NOT NULL,
    CustomerId varchar(255) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Vehicles_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE VisitTypes (
    Id varchar(255) NOT NULL,
    Name varchar(255) NOT NULL,
    Description varchar(255),
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Visits (
    Id varchar(255) NOT NULL,
    Date datetime(6) NOT NULL,
    Description varchar(255),
    VehicleId varchar(255) NOT NULL,
    CustomerId varchar(255) NOT NULL,
    MechanicId varchar(255) NOT NULL,
    VisitTypeId varchar(255) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Visits_Vehicles_VehicleId FOREIGN KEY (VehicleId) REFERENCES Vehicles (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Visits_Customers_CustomerId FOREIGN KEY (CustomerId) REFERENCES Customers (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Visits_Mechanics_MechanicId FOREIGN KEY (MechanicId) REFERENCES Mechanics (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Visits_VisitTypes_VisitTypeId FOREIGN KEY (VisitTypeId) REFERENCES VisitTypes (Id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE Services (
    Id varchar(255) NOT NULL,
    Description varchar(255) NOT NULL,
    Price decimal(65,30) NOT NULL,
    VisitId varchar(255) NOT NULL,
    MechanicId varchar(255) NOT NULL,
    PRIMARY KEY (Id),
    CONSTRAINT FK_Services_Visits_VisitId FOREIGN KEY (VisitId) REFERENCES Visits (Id) ON DELETE RESTRICT,
    CONSTRAINT FK_Services_Mechanics_MechanicId FOREIGN KEY (MechanicId) REFERENCES Mechanics (Id) ON DELETE RESTRICT
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE InventoryItems (
    Id varchar(255) NOT NULL,
    Name varchar(255) NOT NULL,
    PartNumber varchar(255) NOT NULL,
    Location varchar(255),
    CurrentStock int NOT NULL,
    MinimumStock int NOT NULL,
    PRIMARY KEY (Id)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

CREATE TABLE InventoryOperations (
    Id varchar(255) NOT NULL,
    Date datetime(6) NOT NULL,
    OperationType varchar(255) NOT NULL,
    Quantity int NOT NULL,
    InventoryItemId varchar(255) NOT NULL,
    ServiceId varchar(255),
    PRIMARY KEY (Id),
    CONSTRAINT FK_InventoryOperations_InventoryItems_InventoryItemId FOREIGN KEY (InventoryItemId) REFERENCES InventoryItems (Id) ON DELETE CASCADE,
    CONSTRAINT FK_InventoryOperations_Services_ServiceId FOREIGN KEY (ServiceId) REFERENCES Services (Id) ON DELETE SET NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

-- Copy data from the old database
INSERT INTO Bakalauras_New.AspNetUsers SELECT * FROM Bakalauras.AspNetUsers;
INSERT INTO Bakalauras_New.Customers SELECT * FROM Bakalauras.Customers;
INSERT INTO Bakalauras_New.Mechanics SELECT * FROM Bakalauras.Mechanics;
INSERT INTO Bakalauras_New.Vehicles SELECT * FROM Bakalauras.Vehicles;
INSERT INTO Bakalauras_New.VisitTypes SELECT * FROM Bakalauras.VisitTypes;
INSERT INTO Bakalauras_New.Visits SELECT * FROM Bakalauras.Visits;
INSERT INTO Bakalauras_New.Services SELECT * FROM Bakalauras.Services;
INSERT INTO Bakalauras_New.InventoryItems SELECT * FROM Bakalauras.InventoryItems;
INSERT INTO Bakalauras_New.InventoryOperations SELECT * FROM Bakalauras.InventoryOperations;

-- Drop the old database
DROP DATABASE Bakalauras;

-- Rename the new database
RENAME DATABASE Bakalauras_New TO Bakalauras; 