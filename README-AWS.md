# AWS Deployment

## Required GitHub Secrets

Set these secrets in your repository settings (`Settings > Secrets and variables > Actions`):

- `AWS_ACCESS_KEY_ID`
- `AWS_SECRET_ACCESS_KEY`
- `AWS_REGION` (optional, currently hardcoded to `ap-southeast-1` in workflow)
- `ECR_REGISTRY` (for example: `229068445884.dkr.ecr.ap-southeast-1.amazonaws.com`)
- `ECR_REPOSITORY` (for example: `jobs-execution`)
- `ECS_CLUSTER`
- `ECS_SERVICE`

## Build and Deploy Flow

The workflow in `.github/workflows/deploy-aws.yml` does the following:

1. Checks out the repo.
2. Configures AWS credentials from GitHub Secrets.
3. Logs into ECR.
4. Builds the Docker image from `simplified_cloud_job_execution_backend/Dockerfile.aws`.
5. Pushes the image to ECR.
6. Forces an ECS service deployment.

## ECS Setup Notes

- The container listens on port `5000`.
- The backend serves the SPA from `wwwroot`.
- The task should use the DB connection string from Secrets Manager or task environment variables.

## RDS/PostgreSQL

Set your DB connection string in ECS as an environment variable:

`ConnectionStrings__DefaultConnection=Host=<rds-endpoint>;Port=5432;Database=jobsystem;Username=<user>;Password=<password>`

For production, use AWS Secrets Manager and reference the secret in task definition.
