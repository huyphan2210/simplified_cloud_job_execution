# Simplified Cloud Job Execution

A small cloud job execution system with backend APIs, a React frontend, and AWS execution integration.

## What this repo contains

- `simplified_cloud_job_execution_backend/` - .NET 8 backend API, Postgres persistence, S3/SQS/SSM AWS integration
- `simplified_cloud_job_execution_frontend/` - React frontend served by nginx
- `compose.yml` - local Docker Compose stack for backend, frontend, and database
- `docs/` - architecture, API, and design deliverables

> AWS deployment is present in `.github/workflows/deploy-aws.yml`, but this README focuses on local setup and evaluation.

## Prerequisites

- Docker and Docker Compose
- Git

## Quick start with Docker

1. Open a terminal at the repository root.
2. Run:

```bash
docker compose up --build
```

3. Open the app in a browser:

- Frontend UI: `http://localhost`
- Backend API: `http://localhost:5000`
- Swagger (backend dev only): `http://localhost:5000/swagger`

The Docker stack includes:

- `db` - PostgreSQL 17
- `backend` - .NET API service
- `frontend` - built React app served by nginx

## Folder structure

Top-level layout:

```
compose.yml
README.md
README-AWS.md
docs/
simplified_cloud_job_execution_backend/
simplified_cloud_job_execution_frontend/
```

Backend (key paths):

```
simplified_cloud_job_execution_backend/
  Program.cs
  Dependencies.cs
  Controllers/
  Infrastructure/
  Services/
  Repositories/
```

Frontend (key paths):

```
simplified_cloud_job_execution_frontend/
  Dockerfile.nginx
  nginx.conf
  src/
  package.json
```

## Environment configuration

The backend uses `simplified_cloud_job_execution_backend/.env` for environment settings when running via Docker Compose.

Key settings include:

- `ASPNETCORE_ENVIRONMENT=Development`
- `ConnectionStrings__DefaultConnection` for Postgres
- `AWS_ACCESS_KEY_ID`, `AWS_SECRET_ACCESS_KEY`, `AWS_REGION`
- `Sqs__QueueUrl`
- `S3__BucketName`
- `Ssm__InstanceId`

> For local evaluation, the app can still run without valid AWS resources, however EC2 execution, S3 uploads, and SQS processing will require proper AWS configuration.

## Deployment status

The repository contains a GitHub Actions workflow (`.github/workflows/deploy-aws.yml`) to build and push the backend image to ECR and trigger an ECS deployment. This workflow has not been verified against real AWS resources in this assessment — deployment to AWS has not been completed here.

## Assumptions and decisions

- Jobs are created as `Queued`, then execution is performed by a background worker polling SQS.
- The EC2 execution step is intentionally simple: an SSM command writes a job payload to `/tmp` and returns it.
- Output is stored in S3 using a clear prefix structure: `jobs/{jobId}/inputs/...` and `jobs/{jobId}/outputs/...`.
- Billing is calculated after completion and uses a simple per-minute formula:
  - `cpu-small` = 1 credit/min
  - `cpu-large` = 3 credits/min
  - `gpu` = 8 credits/min
- The frontend is minimal and demonstrates submission, job list, and job detail/billing.
- `compose.yml` is the recommended local run method.

## Deliverables

- `README.md` - this file
- `docs/architecture.md` - architecture overview and component flow
- `docs/api.md` - API endpoints and usage
- `docs/design-note.md` - design choices, production evolution, and duplicate billing handling

## Notes

- The repo includes AWS deployment automation in `.github/workflows/deploy-aws.yml`, but the current deliverables focus on local execution.
- Swagger API documentation is available at `http://localhost:5000/swagger` when the backend is running in Development.
