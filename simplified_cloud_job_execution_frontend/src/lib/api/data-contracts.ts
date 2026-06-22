/* eslint-disable */
/* tslint:disable */
// @ts-nocheck
/*
 * ---------------------------------------------------------------
 * ## THIS FILE WAS GENERATED VIA SWAGGER-TYPESCRIPT-API        ##
 * ##                                                           ##
 * ## AUTHOR: acacode                                           ##
 * ## SOURCE: https://github.com/acacode/swagger-typescript-api ##
 * ---------------------------------------------------------------
 */

export enum JobStatus {
  Queued = "Queued",
  Running = "Running",
  Completed = "Completed",
  Failed = "Failed",
}

export enum ComputeType {
  CpuSmall = "CpuSmall",
  CpuLarge = "CpuLarge",
  Gpu = "Gpu",
}

export interface BillingSummaryResponse {
  /** @format uuid */
  jobId?: string;
  jobName?: string | null;
  project?: Project;
  computeType?: ComputeType;
  status?: JobStatus;
  /** @format double */
  executionDurationSeconds?: number | null;
  /** @format double */
  creditCost?: number | null;
  inputFileReference?: string | null;
  outputFileReference?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface CompleteJobResponse {
  /** @format uuid */
  jobId?: string;
  status?: string | null;
  /** @format double */
  creditCost?: number | null;
}

export interface CreateJobResponse {
  /** @format uuid */
  jobId?: string;
  status?: string | null;
}

export interface ErrorResponse {
  errorCode?: string | null;
  message: string | null;
}

export interface GetJobStatusResponse {
  /** @format uuid */
  jobId?: string;
  jobName?: string | null;
  /** @format uuid */
  projectId?: string;
  project?: Project;
  computeType?: ComputeType;
  status?: JobStatus;
  inputFileName?: string | null;
  inputFileReference?: string | null;
  outputFileReference?: string | null;
  /** @format double */
  executionDurationSeconds?: number | null;
  /** @format double */
  creditCost?: number | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}

export interface PagedJobsResponse {
  jobs?: GetJobStatusResponse[] | null;
  /** @format int32 */
  currentPage?: number;
  /** @format int32 */
  totalPage?: number;
  /** @format int32 */
  pageSize?: number;
}

export interface Project {
  /** @format uuid */
  id?: string;
  projectName: string | null;
  description?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
  isDeleted?: boolean;
}

export interface ProjectResponse {
  /** @format uuid */
  id?: string;
  projectName?: string | null;
  description?: string | null;
  /** @format date-time */
  createdAt?: string;
  /** @format date-time */
  updatedAt?: string | null;
}
