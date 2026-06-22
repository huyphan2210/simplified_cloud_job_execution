# API Documentation

## Base URL

- Local backend: `http://localhost:5000`
- API prefix: `/api`

Interactive API docs are available via Swagger UI when the backend runs in Development: `http://localhost:5000/swagger`

## Endpoints

### Create Job

- `POST /api/jobs`
- Content type: `multipart/form-data`
- Request fields:
  - `JobName` (string, required)
  - `ProjectId` (GUID, required)
  - `ComputeType` (enum, required)
  - `InputFile` (file, optional)

- Response:
  - `201 Created`
  - `CreateJobResponse`

### Get Job Status

- `GET /api/jobs/{jobId}`
- Response:
  - `200 OK`
  - `GetJobStatusResponse`

### Complete Job

- `POST /api/jobs/{jobId}:complete`
- Executes the queued job if still in `Queued` state.
- Response:
  - `200 OK`
  - `CompleteJobResponse`

### List Jobs

- `GET /api/jobs`
- Query params:
  - `page` (int)
  - `chunk` (int)
- Response:
  - `200 OK`
  - `PagedJobsResponse`

### Billing Summary

- `GET /api/jobs/{jobId}/billing`
- Response:
  - `200 OK`
  - `BillingSummaryResponse`

### Projects

- `GET /api/projects`
- Response:
  - `200 OK`
  - `ProjectResponse[]`

## Response models

### `CreateJobResponse`

- `jobId` (GUID)
- `status` (string)

### `CompleteJobResponse`

- `jobId` (GUID)
- `status` (string)
- `creditCost` (decimal?)

### `PagedJobsResponse`

- `jobs` (array of `GetJobStatusResponse`)
- `currentPage` (int)
- `totalPage` (int)
- `pageSize` (int)

### `GetJobStatusResponse`

- `jobId` (GUID)
- `jobName` (string)
- `projectId` (GUID)
- `project` (object)
- `computeType` (enum)
- `status` (enum)
- `inputFileName` (string)
- `inputFileReference` (string?)
- `outputFileReference` (string?)
- `executionDurationSeconds` (double?)
- `creditCost` (decimal?)
- `createdAt` (DateTime)
- `updatedAt` (DateTime?)

### `BillingSummaryResponse`

- `jobId` (GUID)
- `jobName` (string)
- `project` (object)
- `computeType` (enum)
- `status` (enum)
- `executionDurationSeconds` (double?)
- `creditCost` (decimal?)
- `inputFileReference` (string?)
- `outputFileReference` (string?)
- `createdAt` (DateTime)
- `updatedAt` (DateTime?)

## Notes

- The frontend uses generated API client code under `simplified_cloud_job_execution_frontend/src/lib/api`.
- Swagger UI is available at `http://localhost:5000/swagger` when the backend is running in Development.
