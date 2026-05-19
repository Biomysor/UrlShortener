# Introduction

**UrlShortener** is a distributed web application for creating short URLs, redirecting users by short codes, caching redirect data, collecting analytics, and sending notifications.

The system is built with **ASP.NET Core**, **.NET 9**, **Entity Framework Core**, **SQL Server**, **Redis**, and **RabbitMQ**.

The project follows a layered architecture with separate projects for API, application logic, domain models, infrastructure, messaging contracts, analytics, notifications, and unit testing.

## Main Features

- User registration and login
- Password hashing with BCrypt
- JWT authentication
- Creating short URLs
- Redirecting users by short code
- Redis caching for faster redirects
- RabbitMQ messaging between services
- Analytics microservice for redirect statistics
- Notifications microservice for Telegram messages
- Unit tests for handlers, services, and domain logic

## Architecture Overview

| Project | Description |
|---|---|
| `UrlShortener.Api` | Main REST API with controllers and Swagger |
| `UrlShortener.Application` | CQRS handlers, interfaces, validation and business logic |
| `UrlShortener.Domain` | Domain entities, value objects and domain errors |
| `UrlShortener.Infrastructure` | Database, repositories, Redis cache, JWT and RabbitMQ publisher |
| `UrlShortener.Messaging.Contracts` | Shared integration events between services |
| `UrlShortener.AnalyticsService` | Microservice that stores redirect statistics |
| `UrlShortener.NotificationsService` | Microservice that sends Telegram notifications |
| `UrlShortener.UnitTests` | Unit tests for application and domain logic |

## Request Flow

### Creating a short URL

1. User sends a request to `POST /url/shortenUrl`.
2. `ShortenUrlCommandHandler` checks if the long URL already exists.
3. If the URL is new, the system creates a `Url` entity.
4. A short code is generated.
5. The URL is saved to SQL Server.
6. Redirect data is saved to Redis cache.
7. `UrlCreatedEvent` is published to RabbitMQ.
8. Notifications service receives the event and sends a Telegram message.

### Redirecting by short code

1. User opens `/r/{code}`.
2. `RedirectQueryHandler` checks Redis cache.
3. If data exists in cache, the original URL is returned immediately.
4. If cache is empty, the URL is loaded from SQL Server and saved to Redis.
5. `UrlRedirectedEvent` is published to RabbitMQ.
6. Analytics service receives the event and updates redirect statistics.

## Technologies

- ASP.NET Core
- .NET 9
- Entity Framework Core
- SQL Server
- Redis
- RabbitMQ
- MediatR
- ErrorOr
- Mapster
- BCrypt
- JWT
- Swagger
- xUnit
- Moq
- FluentAssertions
- DocFX