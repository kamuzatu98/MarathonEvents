# RaceDay / MarathonEvents API

A containerized ASP.NET Core RESTful API built for managing marathon event registrations, categories, and participant enrollments.

## 🚀 Features

* **Event & Category Management**: Endpoints for creating and retrieving marathon events and categories.
* **Enrollments**: Allows participants to register for specific events.
* **OpenAPI / Swagger Documentation**: Interactive API testing interface powered by Swashbuckle.
* **Role-Based Authentication**: Custom authorization and session management.

## 🛠️ Tech Stack

* **Framework**: ASP.NET Core
* **Database & ORM**: SQL Server via Entity Framework Core
* **Security**: BCrypt.Net-Next for password hashing
* **Documentation**: Swashbuckle / Swagger UI

## 🏁 Getting Started

### Prerequisites

* [.NET SDK](https://dotnet.microsoft.com/download)
* SQL Server / LocalDB

### Running the API Locally

1. **Clone the repository:**
   ```bash
   git clone [https://github.com/kamuzatu98/MarathonEvents.git](https://github.com/kamuzatu98/MarathonEvents.git)
   cd MarathonEvents
