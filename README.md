# CQRS Pattern with .NET

This repository demonstrates a practical implementation of the **CQRS (Command Query Responsibility Segregation)** pattern using .NET, along with **event-driven architecture components** such as Debezium and Kafka.

---

## ❗ Problem

Traditional CRUD-based systems often lead to:

* Complex and hard-to-maintain domain logic
* Performance bottlenecks under heavy read/write load
* Tight coupling between read and write operations

---

## ✅ Solution

This project applies **CQRS** to separate:

* **Commands (Write operations)** → modify state
* **Queries (Read operations)** → return data

Additionally, it integrates **Change Data Capture (CDC)** using Debezium to enable event-driven data flow.

---

## 🧠 Architecture

![Architecture Diagram](https://github.com/user-attachments/assets/4f2f6f08-1cd7-4a9e-8ba8-8079cf95ec4d)

### High-Level Flow

1. Client sends request to API
2. API delegates to **MediatR**
3. Command/Query handlers execute logic
4. Data is persisted in database
5. Changes are captured by **Debezium**
6. Events can be published to Kafka (optional extension)

---

## 🔁 CQRS Flow

### Command Flow

```
Request → Command → Handler → Database
```

### Query Flow

```
Request → Query → Handler → Response DTO
```

---

## 🔌 Debezium Connector Setup

Debezium is used to capture database changes and enable event-driven integration.

### Connector Endpoint

```
http://localhost:8083/connectors
```

### Sample Configuration

```json
{
  "name": "PostgresConnector",
  "config": {
    "connector.class": "io.debezium.connector.postgresql.PostgresConnector",
    "tasks.max": "1",
    "topic.prefix": "datatransfer",
    "database.hostname": "host.docker.internal",
    "database.port": "5432",
    "database.user": "postgres",
    "database.password": "P@ssw0rd!",
    "database.dbname": "Products",
    "database.server.name": "host.docker.internal",
    "plugin.name": "pgoutput",
    "name": "PostgresConnector"
  }
}
```

![Debezium Connector](https://github.com/user-attachments/assets/20085f2c-cd58-460f-a8f1-66762d42f1fa)

---

## 🌐 API Gateway

Swagger UI is available at:

```
https://localhost:7000/swagger/index.html
```

![Gateway](https://github.com/user-attachments/assets/2a17ec5a-e9e1-4f33-93ef-bb5470ff4b5d)

---

## ⚙️ Tech Stack

* .NET / ASP.NET Core
* MediatR
* PostgreSQL
* Debezium (CDC)
* Kafka (optional integration)
* Docker

---

## 🚀 How to Run

1. Start infrastructure (Docker Compose)
2. Run API project
3. Configure Debezium connector
4. Test endpoints via Swagger

---

## 📌 Notes

* This project focuses on **CQRS fundamentals + CDC integration**
* Can be extended with:

  * Outbox Pattern
  * Distributed tracing (OpenTelemetry)
  * Idempotency handling
  * MongoDB read models

---

## ⭐ Future Improvements

* Add Kafka consumer & projection layer
* Implement Outbox pattern
* Add integration tests with Testcontainers
* Introduce caching (Redis) for query side

---

👉 This repository is intended as a **foundation for building scalable, event-driven systems**.
