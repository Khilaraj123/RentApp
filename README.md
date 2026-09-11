# RentApp

RentApp is a modern peer-to-peer rental marketplace backend built using **Clean Architecture** and **Domain-Driven Design (DDD)** principles in **.NET 10** and **Entity Framework Core**.

## Architectural Principles

The solution follows clean domain-driven architecture:
- **Encapsulated State**: Entities use `private set` and rich domain methods to mutate state, ensuring domain invariants are protected.
- **Value Objects**: Core concepts like `Money`, `Address`, `GeoLocation`, and `RentalPeriod` are modeled as immutable value objects.
- **Aggregate Roots**: Clean boundary enforcement where aggregate roots manage internal child entities and enforce consistency boundaries.
- **Domain Events**: Inter-aggregate communication and side effects are dispatched via Domain Events (e.g., `BookingRequestedEvent`, `BookingCompletedEvent`, `UserRegisteredEvent`).

---

## Core MVP Bounded Contexts

The current codebase is streamlined for a fast, robust **Minimum Viable Product (MVP)**:

### 👤 Identity & Users (`RentApp.Domain.Entities.Users`)
- **ApplicationUser**: ASP.NET Core Identity user aggregate with profile details, status tracking (`IsBlocked`, `IsDeleted`, `IsEnabled`), and user ratings.
- **RefreshToken**: Token management for secure JWT authentication, token rotation, and multi-device session handling.

### 📦 Catalog (`RentApp.Domain.Entities.Listings`, `RentApp.Domain.Entities.Categories`)
- **Listing**: The core aggregate for items available to rent. Manages pricing rules, availability rules, condition, security deposits, and listing policies.
- **Category**: Hierarchical self-referencing category structure for organizing and navigating listings with SEO-friendly slugs.

### 📅 Bookings (`RentApp.Domain.Entities.Bookings`)
- **Booking**: Manages the complete rental handshake and lifecycle:
  - `Requested` ➔ `Confirmed` ➔ `Paid` ➔ `ItemPickedUp` ➔ `ItemReturned` ➔ `Completed` (or `Cancelled`/`Rejected`).
  - Tracks rental period, quantity, item snapshots, security deposits, and status history.

### 💳 Financials (`RentApp.Domain.Entities.Payments`)
- **Payment**: Handles transaction tracking, payment methods, and gateways.
- **Refund**: Manages manual and automated refund workflows.
- **CommissionTransaction & Coupon**: Platform fee accounting and promotional discount mechanics.

### ⭐ Social & Feedback (`RentApp.Domain.Entities.Reviews`, `RentApp.Domain.Entities.Wishlists`)
- **Review**: Ratings and feedback left by renters/owners upon booking completion.
- **Wishlist**: Saved items for future rentals.

---

## Project Structure

```
RentApp/
├── RentApp.Domain/         # Core business logic, Entities, Value Objects, Domain Events, Repository Interfaces
├── RentApp.Application/    # Use cases, DTOs, Application Interfaces, Options
├── RentApp.Infrastructure/ # External service integrations, JWT, Security, Auth implementations
├── RentApp.Persistence/    # EF Core DbContext, Configurations, Migrations, Repository Implementations
└── RentApp.Presentation/   # ASP.NET Core Web/API Controllers, Views, ViewModels, Routing
```

---

## Roadmap & Planned Modules (Phase 2 & Phase 3)

To keep the initial MVP lightweight, focused, and maintainable, the following modules are planned for future phases:

- **💬 Real-Time Messaging**: SignalR-powered chat between renters and owners directly inside the app.
- **📄 Digital Rental Agreements**: Auto-generated PDF contracts with audit trails and electronic signature capture.
- **🔔 In-App Notification Center**: Centralized notification queue supporting multi-channel delivery (In-App, Push, SMS, Email).
- **⚖️ Trust & Dispute Resolution**: Formal conflict arbitration system with evidence upload workflows and administrative resolution tools.
- **🚩 Content Moderation & Reporting**: User and listing reporting system with moderator dashboards.

---

## Technologies

- **.NET 10** (C# 13)
- **Entity Framework Core 10**
- **PostgreSQL / Npgsql**
- **ASP.NET Core Identity**
- **JWT (JSON Web Tokens)**