# Hostel Management System

Welcome to the Hostel Management System project!  
This application is designed to efficiently manage hostel operations including student management, room allocation, booking requests, complaints, staff management, and reports. Built with C# WinForms and integrated with Microsoft SQL Server, the system provides secure role-based hostel management.

---

# Table of Contents

- Authors
- Prerequisites
- Get Started
- Clone the Repository
- Database Configuration
- Project Structure
- ER Diagram
- Database Schema
- UML Use Case Diagram
- Application Screenshots
- Project Report
- Features
- Admin Features
- Staff Features
- Student Features
- Technology Used
- Roadmap
- Lessons Learned
- FAQ
- Feedback
- License
- Acknowledgements
- Contact

---

# Authors

- Md Ryan Haque Jim
- Minhad Karim Emon
- Md. Taqi Tahmid

---

# Prerequisites

Before starting, ensure you have the following installed:

- Visual Studio
- .NET Framework
- Microsoft SQL Server
- SQL Server Management Studio (SSMS)

---

# Get Started

## Clone the Repository

```bash
git clone https://github.com/YOUR_USERNAME/Hostel-Management-System.git
cd Hostel-Management-System
```

---

# Database Configuration

## Create Database

Open SSMS and run:

```sql
CREATE DATABASE Hostel_Management_System;
GO

USE Hostel_Management_System;
GO
```

---

## Execute SQL Tables

Run all SQL table scripts from the database folder.

Suggested order:

1. Roles
2. UserInfo
3. Staff
4. Students
5. Rooms
6. Bookings
7. RequestRoom
8. Complaints

---

## Configure Connection String

Update your `App.config` file:

```xml
<connectionStrings>
  <add name="HostelDB"
       connectionString="Data Source=YOUR_SERVER_NAME;
       Initial Catalog=Hostel_Management_System;
       Integrated Security=True"
       providerName="System.Data.SqlClient"/>
</connectionStrings>
```

Replace:

```text
YOUR_SERVER_NAME
```

with your SQL Server instance name.

Example:

```text
localhost
```

or

```text
.\SQLEXPRESS
```

---

# Project Structure

```text
Hostel-Management-System/
│
├── Hostel_Management_System/
│
├── Database/
│   └── Hostel_Management_System.sql
│
├── Report/
│   └── OOP2_Project_Report_Grp-4.pdf
│
├── Screenshots/
│
└── README.md
```

---

# ER Diagram

![ER Diagram](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/ER%20Diagram.png)

---

# Database Schema

![Database Schema](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/Database%20Schema.png)

---

# UML Use Case Diagram

![UML Diagram](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/UML_Use%20Case.png)

---

# Application Screenshots

## Login Form

![Login Form](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/login.png)

---

## Sign Up Form

![Sign Up Form](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/signup.png)

---

## Admin Dashboard

![Dashboard](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/Dashboard.png)

---

## Staff Dashboard

![Staff Dashboard](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/staffdash.png)

---

## Student Dashboard

![Student Dashboard](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/studentdash.png)

---

## Staff Management Form

![Staff Management](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/staff.png)

---

## Student Management Form

![Student Management](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/student.png)

---

## Room Management Form

![Room Management](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/room.png)

---

## Booking Management Form

![Booking Management](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/booking.png)

---

## Request Room Form

![Request Room](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/request%20room.png)

---

## Complaints Form

![Complaints](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/complaints.png)

---

## Reports Form

![Reports](Hostel%20Managment%20System/Hostel_Management_System/Screenshots/Reports.png)

---

# Features

## Admin Features

- Admin Login Authentication
- Manage Student Information
- Manage Staff Information
- Manage Room Information
- Monitor Booking Requests
- Approve or Reject Room Requests
- View Hostel Reports
- Monitor Complaints
- View Hostel Statistics
- Role-Based Access Control

---

## Staff Features

- Staff Login Authentication
- Manage Students
- Manage Rooms
- Approve Booking Requests
- Search Student Information
- View Complaints

---

## Student Features

- Student Registration and Login
- Request Hostel Room
- View Request Status
- Submit Complaints
- Search Hostel Information

---

# Technology Used

- C#
- WinForms
- ADO.NET
- Microsoft SQL Server
- .NET Framework

---

# Roadmap

## Future Improvements

- Add Email Verification
- Add Room Payment System
- Add Hostel Notice Board
- Add Advanced Search System
- Add Data Export System
- Improve UI Design
- Add Attendance System

---

# Lessons Learned

During this project, we learned:

- Role-Based Authentication
- Database Integration using ADO.NET
- SQL CRUD Operations
- Windows Forms UI Design
- Data Validation Techniques
- Error Handling
- Database Normalization
- Dashboard Management

---

# FAQ

## Q: How can I add rooms?

A: Login as Admin or Staff and use the Room Management form.

---

## Q: How can students request rooms?

A: Login as Student and open the Request Room form.

---

# Feedback

If you have any feedback or suggestions, feel free to create an issue in the repository.

---

# License

This project is developed for academic purposes.

---

# Acknowledgements

Special thanks to:

- American International University-Bangladesh (AIUB)
- Department of Computer Science
- MD. Raihan Talukder

---

# Contact

## Developers

- Md Ryan Haque Jim
- Minhad Karim Emon
- Md. Taqi Tahmid

## GitHub

```text
https://github.com/mdryanhaquejim21
```
