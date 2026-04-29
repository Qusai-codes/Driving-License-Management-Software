# Driving License Management Software

## 📖 Overview
Driving License Management Software is a **Windows Forms** application for managing driver records, license issuance, renewals, detainment, and release workflows. It is designed for administrative use with an intuitive desktop UI and a layered architecture.

## ✨ Key Features
- Manage drivers and people records
- Issue new local and international licenses
- Renew and replace licenses (lost/damaged)
- Detain and release licenses with fines
- Track license history and application status
- Search and filter across records
- Modular multi-project solution (Presentation, Business, DataAccess)

## 🧱 Solution Structure
- `Presentation` — Windows Forms UI
- `Business` — domain logic and services
- `DataAccess` — SQL Server data access
- `Contracts` — DTOs and shared models

## ✅ Requirements
- .NET Framework 4.8
- SQL Server (LocalDB or full instance)
- Visual Studio 2022+ (recommended)

## ▶️ Getting Started
1. Open the solution in Visual Studio.
2. Update the connection string in the data access layer (if applicable).
3. Build the solution.
4. Run the `Presentation` project.

## 🔎 Notes
- This project targets **.NET Framework 4.8**.
- Ensure your database schema matches the queries in `DataAccess`.

## 📄 License
This project is provided for educational and internal use.