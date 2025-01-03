# Play.Microservices

![Microservices Architecture](https://img.shields.io/badge/microservices-DDD-blue)
![ASP.NET Core](https://img.shields.io/badge/ASP.NET-Core-orange)
![Docker](https://img.shields.io/badge/Docker-Containerization-lightblue)

Play.Microservices is a sample project demonstrating a microservices-based architecture built with modern technologies such as Redis, MongoDB, PostgreSQL, ASP.NET Core, and Docker. The architecture follows the Tactical Domain-Driven Design (DDD) principles.

## Features
- **Microservices Architecture**: Decoupled services to handle specific business domains.
- **Tactical DDD**: Aggregate roots, value objects, and domain events for domain modeling.
- **Database Choices**:
  - Redis for caching.
  - MongoDB for NoSQL storage.
  - PostgreSQL for relational data.
- **Containerization**: Fully containerized using Docker.
- **Event-Driven Communication**: Using message brokers for asynchronous communication.
- **Scalable and Resilient**: Designed with scalability and fault tolerance in mind.

---

## Technologies Used
- **Backend**: ASP.NET Core
- **Databases**:
  - Redis
  - MongoDB
  - PostgreSQL
- **Domain Modeling**: Tactical Domain-Driven Design (DDD)
- **Containerization**: Docker & Docker Compose

---

## Architecture Overview
The system is based on a microservices architecture with the following principles:

- Each service is responsible for a specific bounded context.
- Communication between services is event-driven (e.g., through RabbitMQ or another broker).
- API Gateway for routing requests to the correct service.
- Services are deployed independently using Docker containers.

### Key Components:
1. **Catalog Service**
   - Manages the product catalog.
   - MongoDB as its data store.

2. **Order Service**
   - Handles orders and transactions.
   - PostgreSQL for relational data storage.

3. **Inventory Service**
   - Tracks product inventory levels.
   - Redis for caching.

4. **Gateway**
   - Acts as the single entry point for all clients.

5. **Event Bus**
   - Ensures reliable communication between services.

---

## Prerequisites
Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [MongoDB](https://www.mongodb.com/try/download/community)
- [PostgreSQL](https://www.postgresql.org/download/)
- [Redis](https://redis.io/download)

---

## Getting Started
### 1. Clone the repository:
```bash
git clone https://github.com/yourusername/play.microservices.git
cd play.microservices
```

### 2. Build and Run using Docker Compose:
```bash
docker-compose up --build
```

### 3. Access the Services:
- API Gateway: `http://localhost:5000`
- Catalog Service: `http://localhost:5001`
- Order Service: `http://localhost:5002`
- Inventory Service: `http://localhost:5003`

---

## Project Structure
```
play.microservices/
├── src/
│   ├── CatalogService/         # Catalog service
│   ├── OrderService/           # Order service
│   ├── InventoryService/       # Inventory service
│   └── Gateway/                # API Gateway
├── docker-compose.yml          # Docker Compose configuration
├── README.md                   # Project documentation
└── LICENSE                     # License file
```

---

## Configuration
### Environment Variables
Each service has its own `appsettings.json` and supports overriding settings using environment variables. Refer to the `.env` file for Docker Compose to customize these values.

### Example `.env` File:
```env
# MongoDB
MONGO_CONNECTION_STRING=mongodb://mongo:27017

# PostgreSQL
POSTGRES_CONNECTION_STRING=Host=postgres;Database=playdb;Username=postgres;Password=password

# Redis
REDIS_CONNECTION_STRING=redis:6379
```

---

## Testing
Each service has unit and integration tests. Run the tests using the following command:

```bash
dotnet test
```

---

## Contributing
Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a new branch.
3. Commit your changes.
4. Submit a pull request.

---

## License
This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## Contact
For inquiries or support, please contact [your.email@example.com](mailto:your.email@example.com).
