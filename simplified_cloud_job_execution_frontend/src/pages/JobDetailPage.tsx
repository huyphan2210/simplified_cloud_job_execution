import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { api } from "../lib/api/ApiService";
import {
  Button,
  Card,
  Descriptions,
  Layout,
  Space,
  Spin,
  Typography,
} from "antd";
import type {
  BillingSummaryResponse,
  GetJobStatusResponse,
} from "../lib/api/data-contracts";

function JobDetailPage() {
  const { jobId } = useParams();
  const navigate = useNavigate();
  const [job, setJob] = useState<GetJobStatusResponse | null>(null);
  const [billing, setBilling] = useState<BillingSummaryResponse | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      if (!jobId) return;
      setLoading(true);
      try {
        const [jobRes, billingRes] = await Promise.all([
          api.jobsDetail(jobId),
          api.jobsBillingList(jobId),
        ]);

        if (jobRes.data) {
          setJob(jobRes.data);
        }

        if (billingRes.data) {
          setBilling(billingRes.data);
        }
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [jobId]);

  return (
    <Layout style={{ minHeight: "100vh", padding: 24 }}>
      <div
        style={{
          display: "flex",
          justifyContent: "space-between",
          alignItems: "center",
          marginBottom: 24,
        }}
      >
        <div>
          <Typography.Title level={2} style={{ margin: 0 }}>
            Job Detail
          </Typography.Title>
          <Typography.Text type="secondary">{`Job ${jobId}`}</Typography.Text>
        </div>
        <Button onClick={() => navigate("/")}>Back</Button>
      </div>

      <Spin spinning={loading}>
        {job ? (
          <Space direction="vertical" size="large" style={{ width: "100%" }}>
            <Card title="Job Summary">
              <Descriptions column={1} bordered>
                <Descriptions.Item label="Job ID">
                  {job.jobId}
                </Descriptions.Item>
                <Descriptions.Item label="Job Name">
                  {job.jobName}
                </Descriptions.Item>
                <Descriptions.Item label="Status">
                  {job.status}
                </Descriptions.Item>
                <Descriptions.Item label="Compute Type">
                  {job.computeType}
                </Descriptions.Item>
                <Descriptions.Item label="Input File">
                  {job.inputFileName}
                </Descriptions.Item>
                <Descriptions.Item label="Input File Reference">
                  {job.inputFileReference}
                </Descriptions.Item>
                <Descriptions.Item label="Output File Reference">
                  {job.outputFileReference}
                </Descriptions.Item>
                <Descriptions.Item label="Created At">
                  {job.createdAt}
                </Descriptions.Item>
                <Descriptions.Item label="Updated At">
                  {job.updatedAt}
                </Descriptions.Item>
              </Descriptions>
            </Card>

            <Card title="Billing Summary">
              {billing ? (
                <Descriptions column={1} bordered>
                  <Descriptions.Item label="Credit Cost">
                    {billing.creditCost}
                  </Descriptions.Item>
                  <Descriptions.Item label="Execution Duration">
                    {billing.executionDurationSeconds}
                  </Descriptions.Item>
                  <Descriptions.Item label="Execution Duration">
                    {billing.executionDurationSeconds}
                  </Descriptions.Item>
                </Descriptions>
              ) : (
                <Typography.Text type="secondary">
                  Billing data unavailable
                </Typography.Text>
              )}
            </Card>
          </Space>
        ) : (
          <Typography.Text type="danger">Job not found.</Typography.Text>
        )}
      </Spin>
    </Layout>
  );
}

export default JobDetailPage;
