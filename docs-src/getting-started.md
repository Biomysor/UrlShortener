# Getting Started

## Requirements

Before running the project, make sure you have installed:

- .NET 9 SDK
- SQL Server
- Docker
- Git

## Run Redis

```bash
docker run --name urlshortener-redis -p 6379:6379 -d redis