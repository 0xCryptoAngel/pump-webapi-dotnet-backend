# Pump Master Backend

This is the backend for **Pump Master**, a multi-tenant pump monitoring system built using **.NET 8**. It supports tenant-based routing via subdomains, secure login, pump data management, and token-based authentication.

---

## 🧩 Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **PostgreSQL**
- **JWT (JSON Web Token)** authentication
- **Subdomain-based multi-tenancy**

---

## 🚀 Features

### ✅ Multi-Tenant Support

- Subdomain extraction via custom middleware (`TenantResolutionMiddleware`)
- Tenant resolution based on `Host` header
- Each tenant has isolated pump data

### 🔐 Authentication

- Login using username/password
- JWT token generation with tenant info
- Secured routes using `[Authorize]`

### 🛠 Pump Management API

- CRUD operations for pumps
- Pumps tied to specific tenants via `TenantId`
- Supports pressure tracking, location, and flow rate

---

## 📁 Project Structure

```
├── Controllers/
│   ├── AuthController.cs        # Handles login
│   └── PumpDataController.cs    # Protected endpoints for pump data
│
├── Data/
│   ├── DbContext/
│   │   └── Pump_DbContext.cs    # EF Core context
│   └── Configurations/
│       └── DataSeeder.cs        # Seeds tenants and users
│
├── Models/
│   ├── Pump.cs
│   ├── Tenant.cs
│   └── User.cs
│
├── Services/
│   ├── Interfaces/
│   │   └── ITokenService.cs
│   └── Implementations/
│       └── TokenService.cs      # Generates JWT token
│
├── Middlewares/
│   └── TenantResolutionMiddleware.cs
│
├── Migrations/                  # EF Core Migrations
├── Program.cs                  # Entry point and middleware pipeline
├── appsettings.json
```

---

## ⚙️ Setup Instructions

### 1. Clone & Navigate

```bash
git clone <repo-url>
cd pump-master-backend
```

### 2. Update Database

Ensure PostgreSQL is running and update connection string in `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=pumpmaster;Username=postgres;Password=yourpassword"
}
```

Then run:

```bash
dotnet ef database update
```

### 3. Run the API

```bash
dotnet run
```

API will run at:

```
http://localhost:5164
```

---

## 🔑 JWT Configuration

Add the following section to your `appsettings.json` to define the secret key and token expiration:

```json
"JwtSettings": {
  "Secret": "your-super-secret-key-goes-here",
  "Issuer": "PumpMaster",
  "Audience": "PumpMasterClient",
  "ExpiryMinutes": 60
}
```

Make sure this key is injected in `TokenService.cs` and read using:

```csharp
_configuration["JwtSettings:Secret"]
```

> 🚨 **Do not commit your secret key to version control.** Use environment variables or secret managers in production.

---

## 🔐 Authentication Flow

### 🔸 Login Endpoint

Send a POST request to the login endpoint **with a JSON body**:

```
POST /api/Auth/login
Content-Type: application/json
Host: tenant1.localhost:5164

{
  "username": "admin",
  "password": "1234"
}
```

This request:

- Resolves the tenant from the subdomain (e.g., `tenant1` from `tenant1.localhost`)
- Finds the corresponding user in that tenant
- Validates the password using BCrypt
- Generates a JWT token containing the username and tenant name

**Successful Response:**

```json
{
	"token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

### 🔸 Accessing Protected Resources

Pass the JWT token using the `Authorization` header:

```
Authorization: Bearer {your_token_here}
```

### 🔸 Profile Endpoint

You can test token validity and view resolved context:

```
GET /api/Auth/profile
Authorization: Bearer {token}
Host: tenant1.localhost:5164
```

**Response:**

```json
{
	"tenant": "tenant1",
	"username": "admin"
}
```

> Note: The middleware must set the subdomain as `HttpContext.Items["Tenant"]`, which is later accessed in controller methods.

---

## 🌐 Multi-Tenancy Setup (Subdomain-based)

### 1. Host Configuration

To simulate subdomain locally:

**Edit `hosts` file:**

```
127.0.0.1 tenant1.localhost
127.0.0.1 tenant2.localhost
```

### 2. Middleware

The `TenantResolutionMiddleware` extracts the subdomain from `Request.Host.Host` and sets it in the context for downstream use.

```csharp
var subdomain = host.Split('.')[0];
context.Items["Tenant"] = subdomain;
```

---

## 🔌 API Endpoints

| Method | Endpoint             | Description          |
| ------ | -------------------- | -------------------- |
| POST   | `/api/Auth/login`    | User login           |
| GET    | `/api/PumpData`      | Get all pumps (auth) |
| GET    | `/api/PumpData/{id}` | Get pump by ID       |
| POST   | `/api/PumpData`      | Create pump          |
| PUT    | `/api/PumpData/{id}` | Update pump          |
| DELETE | `/api/PumpData/{id}` | Delete pump          |

---

## 🧪 Sample Seeded Data

The `DataSeeder` class initializes your database with the following:

### 🔹 Tenants

```csharp
new Tenant {
  Name = "tenant1",
  Email = "alpha@company.com",
  Password = BCrypt.HashPassword("hashedpassword1")
}

new Tenant {
  Name = "tenant2",
  Email = "beta@company.com",
  Password = BCrypt.HashPassword("hashedpassword2")
}
```

### 🔹 Users

```csharp
// Tenant1
new User {
  Username = "alpha_admin",
  Email = "admin@alpha.com",
  Password = BCrypt.HashPassword("hashedpassword1")
}

new User {
  Username = "alpha_user",
  Email = "user@alpha.com",
  Password = BCrypt.HashPassword("hashedpassword2")
}

// Tenant2
new User {
  Username = "beta_admin",
  Email = "admin@beta.com",
  Password = BCrypt.HashPassword("hashedpassword3")
}
```

### 🔹 Pumps

Sample pumps with geo-coordinates for Australia:

- **Pump A1** – Sydney (Centrifugal)
- **Pump A2** – Melbourne (Submersible)
- **Pump A3** – Brisbane (Diaphragm)
- **Pump B1** – Perth (Rotary)
- **Pump B2** – Canberra (Peristaltic)

Each pump includes:

- Flow rate
- Pressure thresholds
- Area block name
- GPS coordinates
- Offset timing

---

## 🛡 Security

- JWT-based authentication
- Token includes tenant claim
- Backend validates tenant and user on each request

---

## 📄 License

MIT
