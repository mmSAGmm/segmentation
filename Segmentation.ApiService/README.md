## Segmentation API Service

Minimal API service for managing **segments**, **properties**, and evaluating segments against stored properties.

### Running the API

- **Prerequisites**: .NET 8/10 SDK, access to the SQLite DB file referenced in `appsettings.json`.
- **Database config**: configured via `SQLite:ConnectionString` in `appsettings.json` (by default `Data Source=c:/Segmentation/test.db;`).
- **Run** (from repo root or project folder):

```bash
dotnet run --project Segmentation.ApiService
```

The API exposes Swagger / OpenAPI in `Development` environment at `/swagger` and `/openapi/v1.json`.

### HTTP Endpoints

#### Admin – Segments (`api/admin/v1/segment`)

- **GET** `api/admin/v1/segment/{id:guid}`  
  - **Description**: Get a single segment by id.  
  - **Response**: `Segment` JSON (see `Segmentation.DomainModels.Segment`).  

- **DELETE** `api/admin/v1/segment/{id:guid}`  
  - **Description**: Delete segment by id.  

- **POST** `api/admin/v1/segment`  
  - **Description**: Create a new segment. Server generates `Id`.  
  - **Body**:

```json
{
  "expression": "x.name == \"John\""
}
```

- **PUT** `api/admin/v1/segment`  
  - **Description**: Update existing segment.  
  - **Body**: full `Segment` object including `id`.  

- **GET** `api/admin/v1/segment/page/{number}/{size}`  
  - **Description**: Paged list of segments.  

- **GET** `api/admin/v1/segment/init`  
  - **Description**: Initialize seed data for segments (idempotent helper).

#### Business – Properties (`api/business/v1/properties`)

- **GET** `api/business/v1/properties/{id}`  
  - **Description**: Get all properties for a given `id`.  
  - **Response**: `Dictionary<string, object>`-like JSON.

- **POST** `api/business/v1/properties/{id}`  
  - **Description**: Replace / upsert a batch of properties for `id`.  
  - **Body example**:

```json
{
  "name": "John",
  "age": 30
}
```

- **PATCH** `api/business/v1/properties/{id}/{property}`  
  - **Description**: Set a single property value.  
  - **Body** (`SetPropertyRequestModel`):

```json
{
  "value": "John"
}
```

- **GET** `api/business/v1/properties/init`  
  - **Description**: Initialize seed data for properties (idempotent helper).

#### Business – Evaluation (`api/business/v1/evaluate`)

- **POST** `api/business/v1/evaluate/{segmentId:guid}/{propertiesId}`  
  - **Description**: Evaluate a segment expression against stored properties.  
  - **Response** (`EvaluateResponseModel`):

```json
{
  "result": true
}
```

`result` can be `true`, `false`, or `null` (e.g., if the segment is missing or evaluation fails and is logged).

### Expression Language

- Expressions are stored on `Segment.Expression` and compiled dynamically.  
- Evaluated against a dynamic object `x` built from saved properties.  
- Example:

```text
x.name == "John" && x.age >= 18
```

Types are inferred from stored JSON values (string, number, bool); make sure your expressions match the underlying types.

### Error Handling & Observability

- Global exception handling via `ExceptionHandlingMiddleware` and ProblemDetails.  
- Evaluation errors are logged via `ILogger<EvaluationService>` and result in `result = null`.  
- Swagger UI is available in Development to explore and test all endpoints.


