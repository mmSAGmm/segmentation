## Segmentation Solution

This repository contains a small segmentation system composed of multiple projects:

- **Segmentation.ApiService** – minimal API used to manage segments, store customer properties, and evaluate segments.
- **Segmentation.Domain**, **Segmentation.DataAccess**, **Segmentation.DomainModels**, **Segmentation.Web**, and related projects that implement the core logic, data access, and UI.

### Getting Started

- Install the latest **.NET SDK** (8 or 10).
- Restore and build the solution from the repository root:

```bash
dotnet restore
dotnet build
```

### API Service

For details on running and using the HTTP API, see:

- **[Segmentation.ApiService README](Segmentation.ApiService/README.md)**

That document describes:

- How to run the API service.
- Configuration (including SQLite connection string).
- Available endpoints for admin, properties, and evaluation, with request/response examples.


