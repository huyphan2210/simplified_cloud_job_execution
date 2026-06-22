# Design Note

## Deployment status

This project includes a CI workflow for AWS (`.github/workflows/deploy-aws.yml`) but it has not been executed successfully against real AWS resources in this assessment. Treat AWS deployment as unverified and follow the "How to evolve" steps below to prepare for production deployment.

## How to evolve this into a production-ready AWS platform

1. Use managed infrastructure
   - Replace local Compose with Terraform / CloudFormation for EKS/Fargate, RDS, S3, SQS, and EC2.
   - Use AWS Secrets Manager for database and AWS credentials.

2. Harden execution
   - Use ECS/Fargate or Lambda for execution instead of direct EC2 SSM when appropriate.
   - Implement retries, dead-letter queues, and observability for SQS workers.

3. Secure the API
   - Add authentication and authorization.
   - Use API Gateway or ALB with TLS.
   - Restrict S3, SQS, and SSM IAM roles to least privilege.

4. Improve reliability
   - Use a durable queue pattern with idempotent execution.
   - Track job events in the database separately from job state.
   - Add alerting for failed executions and queue backlog.

5. Production storage and data lifecycle
   - Use lifecycle rules for S3 job artifacts.
   - Use read replicas or multi-AZ for Postgres.
   - Add data retention policies.

## Preventing duplicate billing if completion runs twice

1. Use idempotent state transitions
   - The backend already checks `if (job.Status != JobStatus.Queued)` before executing.
   - This prevents re-execution for the same job if it has already moved to `Running`, `Completed`, or `Failed`.

2. Mark billing as processed separately
   - The job model includes `BillingProcessed`.
   - Execution sets this flag after cost calculation.
   - Future runs should skip billing when `BillingProcessed` is true.

3. Use transactional state updates
   - Ensure job status update and billing calculations are committed together in one transaction.
   - For production, use database row locking or optimistic concurrency control.

4. Use deduplication keys in SQS
   - If using FIFO queues, set a deduplication ID per job to avoid duplicate message processing.
   - If using standard SQS, use a separate message deduplication store.

## Notes on current implementation

- Job creation and execution are decoupled using SQS and a hosted background worker.
- Execution is intentionally simple: SSM runs a shell command and uploads the output text to S3.
- Billing is computed after successful execution based on elapsed duration.
- This design is sufficient for a focused assignment, but production would require stronger failure and retry handling.
