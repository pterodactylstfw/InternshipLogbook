# 🎓 Internship Logbook Management System

A modern web application designed to streamline the university internship process. It allows students to record daily activities and enables coordinators to monitor progress, validate journals, and generate reports.

## 🚀 Key Features

### 👨‍🎓 Student Module
* **Secure Authentication:** Individual accounts with role-based access.
* **Digital Logbook:** Record daily activities including date, time interval, location, and description.
* **Progress Tracking:** Visual indicators of internship completion status.
* **Word Export:** Automatically generate the official Internship Logbook (DOCX) formatted for printing.

### 👨‍🏫 Coordinator Module
* **Dashboard:** Overview of all assigned students and their current status.
* **Journal Management:** View, edit, or delete student activities (Full CRUD capabilities).
* **Monitoring:** Track student progress (hours worked, skills acquired).

---

## 🛠️ Tech Stack

### Backend
* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Database:** Microsoft SQL Server (via Entity Framework Core)
* **Authentication:** JWT (JSON Web Tokens)
* **Libraries:** `DocX` (for report generation), `AutoMapper`.

### Frontend
* **Framework:** Angular 17+ (Standalone Components)
* **UI Library:** PrimeNG (Material Design components)
* **Styling:** SCSS, Flexbox, CSS Grid
* **State Management:** RxJS Observables

---

## 📋 Prerequisites

Before running the project, ensure you have the following installed:

1.  **Node.js** (v18 or higher) - [Download](https://nodejs.org/)
2.  **.NET 8.0 SDK** - [Download](https://dotnet.microsoft.com/download)
3.  **SQL Server LocalDB** (Included with Visual Studio) OR SQL Server Express.
4.  **Visual Studio 2022** or **VS Code**.

---

## ⚙️ Setup & Installation Guide

### 1. Database & Backend Setup

1.  Navigate to the `InternshipLogbook.API` folder.
2.  Open `appsettings.json` and configure the **ConnectionString** and **JWT Key**:

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InternshipLogbookDB;Trusted_Connection=True;MultipleActiveResultSets=true"
      },
      "Jwt": {
        "Key": "7A912F3B4C5D6E8F9A0B1C2D3E4F5A6B7C8D9E0F1A2B3C4D5E6F7A8B9C0D1E2F",
        "Issuer": "https://localhost:5203",
        "Audience": "https://localhost:5203"
      }
    }
    ```
3.  **Run the Application:**
    Open the terminal in the API folder and run:
    ```bash
    dotnet run
    ```
    *Note: The application is configured to automatically create the database and apply migrations on startup.*

4.  The API will be available at `https://localhost:5203` (Swagger UI at `/swagger`).

### 2. Frontend Setup

1.  Navigate to the `client` (or `frontend`) folder in your terminal.
2.  Install dependencies (required only once):
    ```bash
    npm install
    ```
3.  Start the development server:
    ```bash
    npm start
    ```
    *(Or `ng serve` if you have Angular CLI installed globally).*

4.  Open your browser and navigate to `http://localhost:4200`.

---

## 🔐 Default Credentials (Demo)

If the database seed ran successfully, you can use these accounts:

| Role | Email | Password |
| :--- | :--- | :--- |
| **Coordinator** | `admin@test.com` | `admin123` |
| **Student** | `student@test.com` | `student123` |

> **Note:** If these accounts do not work, please use the **Register** button on the login page to create a new account.

---

## ⚠️ Troubleshooting

**Issue: Database connection error**
* Ensure `SQL Server LocalDB` is running.
* Verify the `DefaultConnection` string in `appsettings.json` matches your local SQL instance.

**Issue: CORS Error in Browser**
* The Angular app expects the API to run on port `5203`. If your API runs on a different port, update `proxy.conf.json` or the service URL in `src/app/services/`.

**Issue: "Full Name" not appearing**
* If you registered a user manually via Swagger or SQL, ensure the `FullName` column in the `Users` table is populated.

---

### Project Structure

```text
InternshipLogbook/
├── InternshipLogbook.API/       # .NET Web API
│   ├── Controllers/             # API Endpoints (Auth, Students, Activities)
│   ├── Models/                  # Database Entities
│   └── Data/                    # EF Core Context & Migrations
│
└── client/                      # Angular Frontend
    └── src/
        └── app/
            ├── components/      # Dashboard, Profile, Login Pages
            ├── services/        # HTTP Services
            └── models/          # TypeScript Interfaces