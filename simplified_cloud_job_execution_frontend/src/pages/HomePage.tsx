import { useEffect, useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";
import { api } from "../lib/api/ApiService";
import {
  Button,
  Layout,
  Space,
  Table,
  Typography,
} from "antd";
import type { ColumnsType, TablePaginationConfig } from "antd/es/table";
import CreateJobModal from "../components/CreateJobModal";
import { type GetJobStatusResponse, type ProjectResponse } from "../lib/api/data-contracts";

const columns: ColumnsType<GetJobStatusResponse> = [
  {
    title: "Job ID",
    dataIndex: "jobId",
    key: "jobId",
    render: (value) => <Typography.Text copyable>{value}</Typography.Text>,
  },
  {
    title: "Job Name",
    dataIndex: "jobName",
    key: "jobName",
  },
  {
    title: "Status",
    dataIndex: "status",
    key: "status",
  },
  {
    title: "Compute Type",
    dataIndex: "computeType",
    key: "computeType",
  },
];

function HomePage() {
  const [data, setData] = useState<GetJobStatusResponse[]>([]);
  const [loading, setLoading] = useState(false);
  const [page, setPage] = useState(1);
  const [chunk, setChunk] = useState(10);
  const [totalPage, setTotalPage] = useState(1);
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [projects, setProjects] = useState<ProjectResponse[]>([]);
  const navigate = useNavigate();

  const loadJobs = async (pageIndex = 1, pageSize = 10) => {
    setLoading(true);
    try {
      const response = await api.jobsList({ page: pageIndex, chunk: pageSize });
      if (response.data) {
        setData(response.data.jobs ?? []);
        setPage(response.data.currentPage ?? 1);
        setChunk(response.data.pageSize ?? 10);
        setTotalPage(response.data.totalPage ?? 1);
      }
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    const fetchJobs = async () => {
      setLoading(true)
      try {
        const response = await api.jobsList({ page: 1, chunk: 10 })
        if (response.data) {
          setData(response.data.jobs ?? [])
          setPage(response.data.currentPage ?? 1)
          setChunk(response.data.pageSize ?? 10)
          setTotalPage(response.data.totalPage ?? 1)
        }
      } finally {
        setLoading(false)
      }
    }

    const fetchProjects = async () => {
      try {
        const res = await api.projectsList();
        if (res.data) setProjects(res.data ?? []);
      } catch {
        // ignore for now
      }
    }

    void fetchJobs()
    void fetchProjects()
  }, []);

  const pagination: TablePaginationConfig = useMemo(
    () => ({
      current: page,
      pageSize: chunk,
      total: totalPage * chunk,
      showSizeChanger: true,
      onChange: (pageIndex, pageSize) => {
        setPage(pageIndex);
        setChunk(pageSize);
        loadJobs(pageIndex, pageSize);
      },
    }),
    [page, chunk, totalPage],
  );

  return (
    <Layout style={{ minHeight: "100vh", padding: 24 }}>
      <div style={{ display: "flex", justifyContent: "space-between", alignItems: "center", marginBottom: 24 }}>
        <div>
          <Typography.Title level={2} style={{ margin: 0 }}>
            Jobs Dashboard
          </Typography.Title>
          <Typography.Text type="secondary">
            Browse jobs, paginate, and create new jobs
          </Typography.Text>
        </div>
        <Button type="primary" onClick={() => setIsModalOpen(true)}>
          Create Job
        </Button>
      </div>

      <Space direction="vertical" size="large" style={{ width: "100%" }}>
        <Table
          rowKey="jobId"
          columns={columns}
          dataSource={data}
          loading={loading}
          pagination={pagination}
          onRow={(record) => ({
            onClick: () => navigate(`/job/${record.jobId}`),
          })}
        />
      </Space>

      <CreateJobModal
        projects={projects}
        open={isModalOpen}
        onClose={() => setIsModalOpen(false)}
        onSuccess={() => {
          setIsModalOpen(false);
          loadJobs(page, chunk);
        }}
      />
    </Layout>
  );
}

export default HomePage;
