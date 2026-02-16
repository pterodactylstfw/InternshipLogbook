# Internship Logbook

A full-stack web application for managing university internship journals. Built for Transilvania University of Brașov (UNITBV), it enables students to digitally record daily internship activities and coordinators to oversee student progress — replacing the traditional paper-based logbook workflow.

## Overview

The system implements a role-based architecture with two distinct user experiences:

- **Students** log in to record their daily internship activities (location, time intervals, tasks performed, skills practiced), track progress visually, and export a fully formatted Word document ready for submission.
- **Coordinators** access a dashboard showing all assigned students, monitor completion status, review individual journals, and manage activity records.

Authentication is handled via JWT tokens with role-based route guards ensuring each user type only accesses their permitted views.

## Features

### Student Portal
- Record daily activities with date picker, time range selectors, and rich text fields
- Full CRUD operations — create, edit, and delete activity entries
- Sortable and filterable activity table with global search
- Skeleton loading states and error recovery with retry
- Responsive layout — table view on desktop, card view on mobile
- One-click Word (DOCX) export using a university-approved template
- Toast notifications and confirmation dialogs for all actions
- Dark mode support with persistent preference

### Coordinator Dashboard
- Overview panel showing total assigned students and completion stats
- Student cards with email, company, and visual progress bars
- Click-through to view any student's full journal in a modal dialog
- Responsive grid layout adapting from multi-column to single-column on mobile

### Authentication
- JWT-based login with role detection (Student / Coordinator)
- Automatic routing based on user role after login
- Protected routes via Angular route guards
- Form validation with inline error messages

## Tech Stack

### Backend

| Technology | Purpose |
|---|---|
| .NET 8 | ASP.NET Core Web API |
| Entity Framework Core | ORM with Code-First migrations |
| SQL Server (LocalDB) | Relational database |
| JWT Bearer | Authentication & authorization |
| OpenXML SDK | Word document generation from templates |

### Frontend

| Technology | Purpose |
|---|---|
| Angular 19 | Standalone components with new control flow (`@if`, `@for`) |
| PrimeNG (Aura) | UI component library with built-in dark mode |
| SCSS | Custom theming (UNITBV brand colors) |
| RxJS | Reactive state management and HTTP handling |

## Prerequisites

- [Node.js](https://nodejs.org/) v18+
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download)
- SQL Server LocalDB (included with Visual Studio) or SQL Server Express
- Angular CLI (`npm install -g @angular/cli`)

## Getting Started

### 1. Backend

```bash
cd InternshipLogbook.API
```

Configure `appsettings.json` with your connection string and JWT secret:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=InternshipLogbookDB;Trusted_Connection=True;MultipleActiveResultSets=true"
  },
  "Jwt": {
    "Key": "YOUR_SECRET_KEY_MIN_64_CHARACTERS",
    "Issuer": "https://localhost:5203",
    "Audience": "https://localhost:5203"
  }
}
```

Run the API (database is created and seeded automatically on first run):

```bash
dotnet run
```

The API will be available at `https://localhost:5203` with Swagger UI at `/swagger`.

### 2. Frontend

```bash
cd internship-logbook-ui
npm install
ng serve
```

Open `http://localhost:4200` in your browser.

## Demo Credentials

| Role | Email | Password |
|---|---|---|
| Student | `student@test.com` | `student123` |
| Coordinator | `admin@test.com` | `admin123` |

## Project Structure

```
InternshipLogbook/
├── InternshipLogbook.API/
│   ├── Controllers/
│   │   ├── AuthController.cs          # Login & JWT generation
│   │   ├── StudentsController.cs      # Student CRUD endpoints
│   │   ├── DailyActivitiesController.cs  # Activity CRUD endpoints
│   │   └── ExportController.cs        # Word document generation
│   ├── Models/                        # EF Core entities (Student, DailyActivity, Company, etc.)
│   ├── Services/
│   │   └── WordExportService.cs       # OpenXML template processing
│   ├── Templates/
│   │   └── Template.docx             # University logbook template
│   └── Data/                          # DbContext & migrations
│
└── internship-logbook-ui/
    └── src/app/
        ├── login-page/                # Authentication page
        ├── student-profile/           # Student journal (activities CRUD)
        ├── coordinator-dashboard/     # Coordinator overview & monitoring
        ├── services/
        │   ├── student.ts             # HTTP service for students & activities
        │   ├── auth/auth.ts           # Authentication service with JWT handling
        │   └── theme.ts               # Dark mode toggle with localStorage persistence
        └── models/                    # TypeScript interfaces
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| `POST` | `/api/Auth/login` | Authenticate and receive JWT |
| `GET` | `/api/Students/{id}` | Get student profile with relations |
| `GET` | `/api/Students/coordinator/{id}` | Get students assigned to coordinator |
| `GET` | `/api/DailyActivities/student/{id}` | List activities for a student |
| `POST` | `/api/DailyActivities/student/{id}` | Create a new activity |
| `PUT` | `/api/DailyActivities/{id}` | Update an existing activity |
| `DELETE` | `/api/DailyActivities/{id}` | Delete an activity |
| `GET` | `/api/Export/word/{id}` | Download generated DOCX logbook |

## Troubleshooting

**Database connection fails**

Ensure SQL Server LocalDB is running. Verify the connection string in `appsettings.json` matches your local instance. You can check with:
```bash
sqllocaldb info MSSQLLocalDB
```

**CORS errors in browser**
The Angular dev server expects the API on port `5203`. If your API runs on a different port, update the `baseURL` in `src/app/services/student.ts`.

**Word export returns 500**
Ensure `Templates/Template.docx` exists in the API project root. The template must contain the expected placeholder tags (`{{StudentName}}`, `{{D_1}}`, etc.).

## License

This project was developed as part of the internship program at Transilvania University of Brașov.