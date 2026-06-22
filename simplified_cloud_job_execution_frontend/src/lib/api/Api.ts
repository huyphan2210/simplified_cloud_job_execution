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

import type {
  BillingSummaryResponse,
  CompleteJobResponse,
  ComputeType,
  CreateJobResponse,
  ErrorResponse,
  GetJobStatusResponse,
  PagedJobsResponse,
  ProjectResponse,
} from "./data-contracts";
import { ContentType, HttpClient, type RequestParams } from "./http-client";

export class Api<
  SecurityDataType = unknown,
> extends HttpClient<SecurityDataType> {
  /**
   * No description
   *
   * @tags Job
   * @name JobsDetail
   * @request GET:/api/jobs/{jobId}
   */
  jobsDetail = (jobId: string, params: RequestParams = {}) =>
    this.request<GetJobStatusResponse, ErrorResponse>({
      path: `/api/jobs/${jobId}`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Job
   * @name JobsCreate
   * @request POST:/api/jobs
   */
  jobsCreate = (
    data: {
      JobName: string;
      /** @format uuid */
      ProjectId: string;
      ComputeType?: ComputeType;
      /** @format binary */
      InputFile?: File;
    },
    params: RequestParams = {},
  ) =>
    this.request<CreateJobResponse, ErrorResponse>({
      path: `/api/jobs`,
      method: "POST",
      body: data,
      type: ContentType.FormData,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Job
   * @name JobsList
   * @request GET:/api/jobs
   */
  jobsList = (
    query?: {
      /**
       * @format int32
       * @default 1
       */
      page?: number;
      /**
       * @format int32
       * @default 10
       */
      chunk?: number;
    },
    params: RequestParams = {},
  ) =>
    this.request<PagedJobsResponse, ErrorResponse>({
      path: `/api/jobs`,
      method: "GET",
      query: query,
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Job
   * @name JobsCompleteCreate
   * @request POST:/api/jobs/{jobId}:complete
   */
  jobsCompleteCreate = (
    jobId: string,
    complete: string,
    params: RequestParams = {},
  ) =>
    this.request<CompleteJobResponse, ErrorResponse>({
      path: `/api/jobs/${jobId}${complete}`,
      method: "POST",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Job
   * @name JobsBillingList
   * @request GET:/api/jobs/{jobId}/billing
   */
  jobsBillingList = (jobId: string, params: RequestParams = {}) =>
    this.request<BillingSummaryResponse, ErrorResponse>({
      path: `/api/jobs/${jobId}/billing`,
      method: "GET",
      format: "json",
      ...params,
    });
  /**
   * No description
   *
   * @tags Projects
   * @name ProjectsList
   * @request GET:/api/projects
   */
  projectsList = (params: RequestParams = {}) =>
    this.request<ProjectResponse[], void>({
      path: `/api/projects`,
      method: "GET",
      format: "json",
      ...params,
    });
}
