CREATE DATABASE Hostel_Management_System;
GO

USE Hostel_Management_System;
GO



CREATE TABLE Roles
(
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(20) NOT NULL UNIQUE
);

INSERT INTO Roles (RoleName)
VALUES ('Admin'), ('Staff'), ('Student');



CREATE TABLE UserInfo
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Gender NVARCHAR(10) NOT NULL,
    RoleId INT NOT NULL,

    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);



CREATE TABLE Staff
(
    StaffId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    Designation NVARCHAR(100),
    Salary DECIMAL(10,2),
    JoinDate DATE DEFAULT GETDATE()
);


CREATE TABLE Students
(
    StudentId INT IDENTITY(1,1) PRIMARY KEY,
    StudentName NVARCHAR(100),
    Department NVARCHAR(100),
    Session NVARCHAR(50),
    Address NVARCHAR(200),
    PhoneNo NVARCHAR(20)
);


CREATE TABLE Rooms
(
    RoomId INT IDENTITY(1,1) PRIMARY KEY,
    RoomNumber NVARCHAR(20) UNIQUE NOT NULL,
    RoomType NVARCHAR(50),
    Capacity INT NOT NULL,
    AvailableBeds INT NOT NULL,
    FloorNo INT
);


CREATE TABLE Bookings
(
    BookingId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    RoomId INT NOT NULL,
    BookingDate DATE DEFAULT GETDATE(),
    BookingStatus NVARCHAR(30),

    FOREIGN KEY (StudentId) REFERENCES Students(StudentId),
    FOREIGN KEY (RoomId) REFERENCES Rooms(RoomId)
);


CREATE TABLE RequestRoom
(
    RequestId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    RequestedRoomType NVARCHAR(50),
    RequestDate DATE DEFAULT GETDATE(),
    RequestStatus NVARCHAR(30),

    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);



CREATE TABLE Complaints
(
    ComplaintId INT IDENTITY(1,1) PRIMARY KEY,
    StudentId INT NOT NULL,
    ComplaintText NVARCHAR(MAX),
    ComplaintDate DATE DEFAULT GETDATE(),
    

    FOREIGN KEY (StudentId) REFERENCES Students(StudentId)
);

SELECT * FROM Roles;
SELECT * FROM UserInfo;
SELECT * FROM Staff;
SELECT * FROM Students;
SELECT * FROM Rooms;
SELECT * FROM Bookings;
SELECT * FROM RequestRoom;
SELECT * FROM Complaints;



DROP TABLE IF EXISTS Complaints;
DROP TABLE IF EXISTS RequestRoom;
DROP TABLE IF EXISTS Bookings;

DROP TABLE IF EXISTS Rooms;
DROP TABLE IF EXISTS Students;
DROP TABLE IF EXISTS Staff;
DROP TABLE IF EXISTS UserInfo;

DROP TABLE IF EXISTS Roles;


