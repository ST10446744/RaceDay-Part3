# 🏃 RaceDay – Event Management System

> **A comprehensive digital event management platform for road running, walking and cycling events.**

RaceDay is a full-stack event management system developed to simplify the planning, administration and management of sporting events.

The platform provides functionality for event organisers and participants through a centralised system consisting of a relational database, RESTful Web API and MVC web application.

The project is developed using an incremental software development approach across three Portfolio of Evidence (PoE) submissions.

---

# 📌 Project Overview

RaceDay aims to provide a structured digital solution for managing the complete lifecycle of sporting events.

The platform supports event creation, participant registrations, category management, results, payments, user profiles, supporting documents and image management.

The system follows a layered architecture, separating the presentation layer, API, business logic and database responsibilities.

Development is divided into three main stages:

| Part | Description | Main Technologies |
|---|---|---|
| Part 1 | System Planning & Database Design | SQL Server, SQL, ERD |
| Part 2 | RESTful API Development | C#, ASP.NET Core, EF Core |
| Part 3 | MVC Application & Cloud Integration | ASP.NET Core MVC, Azure Blob Storage, Docker |

## 🎯 Project Objectives

The main objectives of RaceDay are to:

- Manage user accounts and user roles.
- Provide secure authentication and authorisation.
- Allow organisers to create and manage sporting events.
- Manage event categories.
- Support participant registrations and enrolments.
- Record and manage event results.
- Manage event-related documents and images.
- Support profile pictures and user profile management.
- Record payment information.
- Store event weather information.
- Provide RESTful API functionality.
- Connect the MVC application to the backend API.
- Integrate cloud-based file storage.
- Containerise application components using Docker.
- Maintain a structured and maintainable software architecture.

---

# 📚 PART 1 – System Planning & Database Design

Part 1 establishes the foundation of the RaceDay system through database planning, requirements analysis and API design.

The objective was to design a relational database capable of supporting the functionality required by the event management platform.

## Completed Components

| Component | Description | Status |
|---|---|:---:|
| Database Design | Relational database structure | ✅ |
| SQL Script | Database creation and configuration | ✅ |
| Entity Relationship Diagram | Database relationships and structure | ✅ |
| API Planning | RESTful endpoint specification | ✅ |
| Documentation | Technical project documentation | ✅ |
| GitHub | Source control and repository setup | ✅ |

## 🗄️ Database Design

The RaceDay database uses Microsoft SQL Server as its relational database management system.

The database is designed to maintain relationships between users, events, participants and other event-related information.

### Main Database Entities

- Users
- Events
- Categories
- Enrolments
- Results
- Event Documents
- Payments
- Weather Information

The database design uses primary keys, foreign keys and relational constraints to maintain data integrity.

Entity relationships allow the application to associate participants with events, categories, enrolments and results.

### Database Documentation

**SQL Database Script**

`docs/RaceDay_Database.sql`

**Entity Relationship Diagram**

`docs/RaceDay_ERD.png`

## 🔗 Entity Relationship Diagram

The ERD provides a visual representation of the database structure.

It identifies:

- Database entities.
- Attributes.
- Primary keys.
- Foreign keys.
- Relationships.
- Relationship cardinalities.

The ERD serves as the foundation for the SQL database implementation.

## 🌐 API Endpoint Planning

Part 1 also established the planned RESTful API structure.

The API specification covers:

- Authentication.
- User management.
- Event management.
- Category management.
- Participant enrolments.
- Results.
- Payments.
- Event documents.
- Weather information.

**API Documentation:**

`docs/RaceDay_API_Endpoint_Plan.docx`

---

# ⚙️ PART 2 – RESTful API Development

Part 2 focuses on implementing the backend functionality using C# and ASP.NET Core Web API.

The API provides the central communication layer between the database and the client applications.

It handles incoming HTTP requests, processes application logic, communicates with the database and returns structured responses.

## 🛠️ Backend Technologies

| Technology | Purpose |
|---|---|
| C# | Backend programming language |
| ASP.NET Core Web API | RESTful API development |
| .NET 8 | Application framework |
| Entity Framework Core | Object-relational mapping |
| Microsoft SQL Server | Relational database |
| EF Core Migrations | Database schema management |
| Swagger / OpenAPI | API documentation and testing |
| xUnit | Automated testing |

## 🌐 RESTful API Functionality

The backend implements RESTful endpoints using standard HTTP methods.

| HTTP Method | Purpose |
|---|---|
| GET | Retrieve information |
| POST | Create records |
| PUT | Update existing records |
| DELETE | Remove records |

### Main API Modules

The API is organised into controllers responsible for different areas of the application.

- Authentication Controller
- Categories Controller
- Enrolments Controller
- Events Controller
- Payment Controller
- Profile Controller
- Results Controller
- Images Controller

Each controller is responsible for handling requests associated with its respective functionality.

## 🔐 Authentication & Authorisation

RaceDay implements role-based access control to separate organiser and participant functionality.

### Organiser

Organisers are responsible for managing sporting events.

Functionality includes:

- Creating events.
- Updating event information.
- Managing event categories.
- Viewing participant information.
- Managing event-related information.
- Managing results.

### Participant

Participants interact with the platform to access sporting events.

Functionality includes:

- Creating an account.
- Logging into the platform.
- Viewing available events.
- Registering for events.
- Managing enrolments.
- Viewing relevant results.
- Managing profile information.

Access to protected functionality is controlled through authentication and authorisation.

## 🗄️ Entity Framework Core

Entity Framework Core is used to manage communication between the application and SQL Server.

The project uses a Code-First development approach.

This allows database structures to be represented through C# entity models.

Database migrations are used to introduce and maintain changes to the database schema.

Examples include:

- User model changes.
- Event model changes.
- Category model changes.
- Image path additions.
- Profile picture path additions.

## 🧪 Testing

Automated testing is included to verify application functionality and reduce the possibility of regressions.

The project uses unit testing to test important application components.

Testing focuses on:

- API functionality.
- Application logic.
- Data handling.
- Expected responses.
- Validation and error handling.

## ⚙️ Continuous Integration

GitHub Actions is used to automate development checks.

The workflow supports:

- Building the application.
- Restoring dependencies.
- Running automated tests.
- Identifying build failures.

The purpose is to maintain application quality throughout development.

---

# 🚀 PART 3 – MVC Application, Azure Blob Storage & Docker

Part 3 expands RaceDay beyond a standalone backend API by introducing a separate MVC web application and cloud-based file storage.

The objective is to create a more complete application architecture in which users interact with a web interface while the MVC application communicates with the backend API.

This stage also introduces containerisation to improve application deployment and consistency.

## 🏗️ Part 3 Architecture

The application follows a multi-layer architecture.

```text
             USER
              |
              ▼
    ┌─────────────────────┐
    │    RaceDay MVC      │
    │   ASP.NET Core      │
    │   Web Application   │
    └──────────┬──────────┘
               |
               │ HTTP Requests
               ▼
    ┌─────────────────────┐
    │   RaceDay API       │
    │   ASP.NET Core      │
    │   RESTful Services  │
    └──────────┬──────────┘
               |
        ┌──────┴──────┐
        |             |
        ▼             ▼
┌──────────────┐ ┌───────────────┐
│ SQL Server   │ │ Azure Blob    │
│ Database     │ │ Storage       │
└──────────────┘ └───────────────┘

          Docker
    Application Containers
```

The architecture separates user interaction, backend processing, database operations and cloud-based file storage.

## 🖥️ ASP.NET Core MVC Application

The RaceDay MVC project introduces the presentation layer of the system.

The MVC application provides the user-facing interface through which organisers and participants interact with RaceDay.

The Model-View-Controller architectural pattern separates application responsibilities into three components:

| Component | Responsibility |
|---|---|
| Model | Represents application data |
| View | Displays the user interface |
| Controller | Handles user requests and application flow |

The MVC application communicates with the backend API rather than directly managing database operations.

This separation improves maintainability and allows the backend and frontend to evolve independently.

### MVC Responsibilities

- User-facing application interface.
- Communication with the RaceDay API.
- User registration and login workflows.
- Event information and management interfaces.
- Participant functionality.
- Organiser functionality.
- User profile management.
- Image and profile picture functionality.

## ☁️ Azure Blob Storage Integration

Azure Blob Storage is introduced as the cloud storage solution for application files.

Blob Storage provides scalable object storage for unstructured data such as images and documents.

Within RaceDay, cloud storage supports the management of event-related files and user images.

### Intended Storage Responsibilities

- Event images.
- User profile pictures.
- Supporting event documents.
- Other application-related files.

The database maintains relevant file metadata and storage references, while the actual files are managed through cloud storage.

This approach separates file storage from relational database records.

## 🖼️ Image Management

Part 3 extends the backend with image-related functionality.

The project includes an ImagesController and additional database migrations supporting image paths and profile picture information.

Image management allows the application to associate uploaded images with relevant application records.

Relevant implementation components include:

- ImagesController.
- Image-related services.
- Profile picture functionality.
- Image path database fields.
- Entity Framework Core migrations.

## 🐳 Docker Containerisation

Docker is introduced to support application containerisation.

Containerisation packages an application and its required dependencies into a consistent runtime environment.

This helps reduce differences between development and deployment environments.

Docker is used as part of the deployment architecture for the RaceDay application.

The containerisation approach supports:

- Consistent application environments.
- Easier application deployment.
- Dependency isolation.
- Repeatable builds.
- Improved portability.

## 🔄 Part 3 Development Objectives

| Component | Description |
|---|---|
| MVC Application | Separate ASP.NET Core MVC frontend |
| API Integration | MVC communication with backend API |
| Azure Blob Storage | Cloud-based file storage |
| Image Management | Event and user image functionality |
| Profile Pictures | User profile image support |
| Database Migrations | Additional image-related database fields |
| Docker | Application containerisation |

---

# 🔐 Security Architecture

Security is an important consideration throughout RaceDay development.

The application separates authentication from authorisation.

Authentication establishes the identity of a user, while authorisation determines which resources and operations that user can access.

Security considerations include:

- User authentication.
- Role-based access control.
- Protected API endpoints.
- Input validation.
- Secure password handling.
- Database integrity.
- Controlled access to application resources.
- Secure configuration of connection strings and cloud credentials.

Sensitive credentials should be stored outside source control using appropriate environment configuration or secret-management facilities.

---

# 🧰 Technology Stack

The following technologies form the RaceDay development environment.

| Technology | Application |
|---|---|


