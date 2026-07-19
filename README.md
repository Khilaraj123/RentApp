# RentApp

RentApp is a modern peer-to-peer rental marketplace backend built using **Clean Architecture** and **Domain-Driven Design (DDD)** principles.

## Architecture

The project strictly follows DDD principles in its core domain model:
- **Encapsulated State**: Entities use `private set` and rich domain methods to mutate state, ensuring invariants are always protected.
- **Value Objects**: Core concepts like `Money`, `Address`, `GeoLocation`, and `RentalPeriod` are modeled as immutable value objects.
- **Aggregate Roots**: Logical boundaries are enforced (e.g., `Payment` owns `Refund`, `Report` owns `ReportEvidence`, `Dispute` manages its lifecycle).
- **Domain Events**: Inter-aggregate communication and side effects (like sending notifications or updating external gateways) are handled via Domain Events (`PaymentSucceededEvent`, `ConversationStartedEvent`, `ReportAssignedEvent`, etc.).

## Core Bounded Contexts

### 📦 Catalog
- **Listing**: The core aggregate for items available to rent. Manages pricing rules, availability rules, condition, security deposits, and listing policies.
- **Category**: Self-referencing hierarchical structure for navigating listings using SEO-friendly slugs.

### 📅 Bookings & Agreements
- **Booking**: Manages the rental lifecycle from Request to Active, Completed, or Cancelled.
- **RentalAgreement**: Represents a snapshot of the legal contract and terms exactly as they were agreed upon at the time of booking.

### 💬 Messaging
- **Conversation**: A lightweight, highly scalable aggregate serving as a boundary for inbox logic. Tracks unread counts, preview snippets, and active contexts (`ListingId`, `BookingId`).
- **Message**: Strongly typed messages (Text, Image, System) that dispatch real-time events.
- **MessageAttachment**: Tracks multimedia file uploads sent in chat.

### 💳 Financials
- **Payment**: The financial aggregate root handling transactions, gateways (Stripe, Esewa, Khalti, FonePay), and owning refunds to guarantee invariants.
- **Refund**: A child entity managing manual and automated refund workflows.
- **CommissionTransaction**: Tracks platform accounting and fees.
- **Coupon**: Enforces complex discount rules and usage limits.

### 🛡️ Trust & Safety
- **Report**: Moderation aggregate that handles flagged users, listings, or messages. Owns `ReportEvidence` files and follows a strict administrative workflow.
- **Dispute**: Enforces resolution for conflicts (Damaged Item, Missing Item, etc.) between renters and owners.

### 🔔 Notifications
- **Notification**: Supports multi-channel delivery (Email, Push, In-App) by decoupling delivery status from in-app read status.

## Technologies
- **.NET 10** (C#)
- **Entity Framework Core**