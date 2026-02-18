# Internship Logbook

A full-stack web application for managing university internship journals.  
Built for **Transilvania University of Brașov (UNITBV)**, it enables students to digitally record daily internship activities and coordinators to oversee student progress — replacing the traditional paper-based logbook workflow.

![Angular](https://img.shields.io/badge/Angular-21-dd0031?style=flat&logo=angular)
![.NET](https://img.shields.io/badge/.NET-8.0-512bd4?style=flat&logo=dotnet)
![Docker](https://img.shields.io/badge/Docker-Enabled-2496ed?style=flat&logo=docker)

---

## 🐳 Quick Start (Docker) — Recommended

The easiest way to run the application is using **Docker Compose**.  
This ensures the Database, API, and Frontend work together instantly without installing dependencies locally.

### Prerequisites

- [Docker Desktop](https://www.docker.com/products/docker-desktop/) installed and running.

---

### 1️⃣ Clone the repository

```bash
git clone https://github.com/pterodactylstfw/InternshipLogbook.git
cd InternshipLogbook
```

---

### 2️⃣ Run the application

```bash
docker-compose up --build
```

Wait a few minutes for the images to download and build.  
The database is automatically seeded with demo data on the first run.

---

### 3️⃣ Access the app

- **Frontend (UI):** http://localhost:4200
- **Backend (Swagger):** http://localhost:5203/swagger

---

### 4️⃣ Stop the application

Press `Ctrl + C` in the terminal or run:

```bash
docker-compose down
```

To reset the database completely:

```bash
docker-compose down -v
```

---

## 🛠 Manual Installation (For Development)

If you prefer to run the project without Docker to debug code, follow these steps.

### Prerequisites

- Node.js v20+
- .NET 8.0 SDK
- SQL Server (LocalDB or Express)

---

## 🔹 1. Backend Setup

Navigate to the API folder:

```bash
cd InternshipLogbook/InternshipLogbook.API
```

Update `appsettings.json` connection string if needed (default uses LocalDB).

Run the API:

```bash
dotnet run
```

The API will start at:

```
https://localhost:5203
```

---

## 🔹 2. Frontend Setup

Navigate to the UI folder:

```bash
cd InternshipLogbook/internship-logbook-ui
```

Install dependencies:

```bash
npm install
```

Start the server:

```bash
ng serve
```

Open in browser:

```
http://localhost:4200
```

---

## 🔑 Demo Credentials

The database is automatically populated with these accounts:

| Role        | Email              | Password    |
|------------|-------------------|------------|
| Student    | student@test.com  | student123 |
| Coordinator| admin@test.com    | admin123   |

> Note: The SQL Server `sa` password in Docker is  
> **InternshipLog2026!** (if you need direct DB access).

---

## 🏗 Tech Stack

### 🔹 Backend

| Technology              | Purpose                                     |
|--------------------------|---------------------------------------------|
| .NET 8                  | ASP.NET Core Web API                       |
| Entity Framework Core   | ORM with Code-First migrations             |
| SQL Server              | Relational database (2022 Docker image)    |
| JWT Bearer              | Secure authentication & authorization      |
| OpenXML SDK             | Word document generation from templates    |

---

### 🔹 Frontend

| Technology     | Purpose                                        |
|----------------|------------------------------------------------|
| Angular 21      | Modern framework with standalone components    |
| PrimeNG (Aura) | UI component library with Dark Mode support    |
| Nginx          | High-performance web server (Docker production)|

---

## 📡 API Endpoints

| Method | Endpoint                            | Description                          |
|--------|------------------------------------|--------------------------------------|
| POST   | `/api/Auth/login`                  | Authenticate and receive JWT token  |
| GET    | `/api/Students/{id}`               | Get full student profile            |
| GET    | `/api/Students/coordinator/{id}`   | Get students assigned to coordinator|
| GET    | `/api/DailyActivities/student/{id}`| List all activities for a student   |
| POST   | `/api/DailyActivities/student/{id}`| Create a new activity entry         |
| PUT    | `/api/DailyActivities/{id}`        | Update an existing activity         |
| DELETE | `/api/DailyActivities/{id}`        | Delete an activity                  |
| GET    | `/api/Export/word/{id}`            | Download generated DOCX logbook     |

---

## ❓ Troubleshooting

### 1️⃣ "Connection Refused" when running Docker

SQL Server takes longer to start than the API.  
If the API fails immediately, wait 10 seconds — Docker will automatically restart the API container, and it should connect successfully on the second try.

---

### 2️⃣ "401 Unauthorized" at Login

If you restarted Docker but kept old volumes, the database might be out of sync with your JWT token or empty.

Reset everything:

```bash
docker-compose down -v
docker-compose up --build
```

---

### 3️⃣ Word Export Issues

Ensure the file:

```
Templates/Template.docx
```

is present in the API folder.

Docker handles this automatically via the `Dockerfile COPY` instruction.

---

## 📄 License

Developed for the TSG (Transilvania Star Group) at Transilvania University of Brașov.