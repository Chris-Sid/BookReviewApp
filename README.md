
# 📚 BookReviewApp

An ASP.NET Core 9 web application that allows users to register/login, view/manage books, write reviews, and vote (like/dislike) on reviews. It includes both an MVC UI and a RESTful API with JWT authentication and PostgreSQL persistence followin N-Tier architecture.

---

## 🚀 Features

- 🔐 User registration & login (ASP.NET Identity)
- 📘 Book and review management (MVC + Razor Views)
- 📘 Books Pagination (Offset on Master branch / Keyset on Keyset-Pagination branch)
- 👍👎 One vote per review per user
- ⚙️ RESTful API for books & reviews
- 🧪 Unit tested services & repositories
- 🌐 Swagger UI with JWT token authorization
- 🐘 PostgreSQL database via Docker

---

## 🛠️ Installation Guide

### 🔧 Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- Optional: [Postman](https://www.postman.com/) for API testing

---

### 🐳 Start PostgreSQL with Docker

```bash
docker run -d -p 5433:5432 --name bookreview-postgres \
  -e POSTGRES_USER=bookadmin \
  -e POSTGRES_PASSWORD=SuperSecure123 \
  -e POSTGRES_DB=bookreviewdb \
  postgres:latest
```

---

### ⚙️ Setup Environment Variables

You must set the following JWT & PostgreSQL database configuration as **User environment variables**:
Run as Admin Powershell and type:
```powershell
[System.Environment]::SetEnvironmentVariable("BOOKREVIEW_DB_CONNECTION", "Host=localhost;Port=5433;Database=bookreviewdb;Username=bookadmin;Password=SuperSecure123", "Machine")
[System.Environment]::SetEnvironmentVariable("BookAPP_JWT_ISSUER", "test.gr", "User")
[System.Environment]::SetEnvironmentVariable("BookAPP_JWT_AUDIENCE", "test", "User")
[System.Environment]::SetEnvironmentVariable("BookAPP_JWT_KEY", "justADummyTokenKeyForDummyTest2025!", "User")
[System.Environment]::SetEnvironmentVariable("BookAPP_JWT_EXPIRY_MINUTES", "60", "User")
```

📌 Tip: Restart your IDE or terminal after setting them or even Visual Studio so that it will work.

---

### 🗃️ Apply Migrations & Create Database

From your solution directory:

```bash
dotnet ef migrations add Init --project BookReviewApp.DataAccess --startup-project BookReviewApp.WebUI
dotnet ef database update --project BookReviewApp.DataAccess --startup-project BookReviewApp.WebUI
```

---

## 🔑 API Authentication (JWT)

- Use the `/api/auth/login` endpoint to obtain a **JWT token**.
- Open Swagger UI at: `https://localhost:{PORT}/swagger`
- Click **"Authorize"** at the top right.
- Paste the token as:

```
Bearer {your_token_here}
```

Example:

```
Bearer eyJhbGciOiJIUzI1NiIsInR5cCI...
```

---

## 📫 API Endpoints Overview

| Method | Endpoint                       | Description                        |
|--------|--------------------------------|------------------------------------|
| GET    | `/api/books`                   | List books (with optional filters) |
| GET    | `/api/books/{id}`              | Book details                       |
| POST   | `/api/books`                   | Add new book (Admin only)          |
| GET    | `/api/books/{id}/reviews`      | Get reviews for a book             |
| POST   | `/api/reviews`                 | Add review (Customer only)         |
| POST   | `/api/reviews/{id}/vote`       | Like/Dislike a review              |
| POST   | `/api/auth/login`              | Authenticate and get token         |

---

## ✅ Roles

- **Admin**: Full control over books
- **Customer**: Can review and vote
- **Anonymous**: Read-only access

---

## 🧪 Unit Testing

- Unit tests exist under `BookReviewApp.Tests`
- Services like `BookService`, `ReviewService` are covered using mocks
- Run via:

```bash
dotnet test
```

---

## 🎯 Technologies Used

- ASP.NET Core 9 MVC & API
- Entity Framework Core 9
- PostgreSQL
- Docker
- Swagger / Swashbuckle
- JWT Bearer Authentication
- Razor Views
- XUnit & Moq for unit tests

---
