# Pump Backend API

This is a multi-tenant capable .NET 8 Web API backend for managing pump data. It supports user authentication and provides CRUD operations on pump-related data.

## 🚀 Features

- ✅ Multi-tenant structure support
- ✅ JWT-based authentication and authorization
- ✅ RESTful API endpoints
- ✅ Entity Framework Core for database interaction
- ✅ Environment-specific configuration (Development and Production)
- ✅ Modular codebase: Controllers, Services, Entities, Middleware

---

## 📁 Project Structure

```
Pump-Backend/
├── Controllers/             # API endpoints for pump data and authentication
├── Entities/                # Database entity models
├── Services/                # Business logic and data access
├── Migrations/              # EF Core database migrations
├── Middlewares/            # Custom middleware (e.g., error handling, tenant resolution)
├── Program.cs               # App configuration and middleware setup
├── appsettings.json         # General configuration
├── appsettings.Development.json # Dev-specific config
```

---

## 🔐 Authentication

Authentication is handled using JWT tokens. Users are authenticated based on their username and password, and a tenant identifier.

**Login Endpoint**:

```http
POST /api/auth/login
```

**Payload:**

```json
{
  "tenant": "tenant1",
  "username": "user1",
  "password": "pass123"
}
```

**Response:**
Returns a JWT token if credentials are valid.

---

## 📦 Pump Data API

**GET** `/api/pumpdata`  
Fetches all pump records.

**GET** `/api/pumpdata/{id}`  
Fetches a specific pump by ID.

**POST** `/api/pumpdata`  
Creates a new pump.

**PUT** `/api/pumpdata/{id}`  
Updates an existing pump.

---

## 🛠 Setup Instructions

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A database (e.g., PostgreSQL )

### Configuration

Update your connection string in `appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Your-DB-Connection-Here"
}
```

### Run Migrations

```bash
dotnet ef migrations add InitialCreate
```

```bash
dotnet ef database update
```

### Run the API

```bash
dotnet run
```

---

## 🧪 Testing

You can use tools like [Postman](https://www.postman.com/) or [curl](https://curl.se/) to test endpoints.

---

## 🔄 Environment Switching

This project supports multiple environments through `appsettings.{Environment}.json`. Set the environment with:

```bash
export DOTNET_ENVIRONMENT=Development
```

---

## 📧 Contact

For questions or issues, feel free to reach out to the maintainer.
