import { useState } from "react";
import { api } from "../lib/api/ApiService";
import {
  Button,
  Form,
  Input,
  Modal,
  Select,
  Space,
  Upload,
  message,
} from "antd";
import type { UploadFile } from "antd";
import { ComputeType, type ProjectResponse } from "../lib/api/data-contracts";

interface CreateJobModalProps {
  open: boolean;
  onClose: () => void;
  onSuccess: () => void;
  projects: ProjectResponse[];
}

interface CreateJobFormValues {
  jobName: string;
  projectId: string;
  computeType: ComputeType;
  inputFile?: UploadFile[];
}

function CreateJobModal({ open, onClose, onSuccess, projects }: CreateJobModalProps) {
  const [form] = Form.useForm<CreateJobFormValues>();
  const [submitting, setSubmitting] = useState(false);

  const handleFinish = async (values: CreateJobFormValues) => {
    setSubmitting(true);
    try {
      const createBody = {
        JobName: values.jobName,
        ProjectId: values.projectId,
        ComputeType: values.computeType,
        InputFile:
          values.inputFile && values.inputFile.length > 0
            ? (values.inputFile[0].originFileObj as File)
            : undefined,
      };

      const result = await api.jobsCreate(createBody);
      if (result.data) {
        message.success("Job created successfully");
        onSuccess();
      }
    } catch {
      message.error("Unable to create job.");
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Modal
      open={open}
      title="Create Job"
      onCancel={onClose}
      footer={null}
      destroyOnHidden
    >
      <Form form={form} layout="vertical" onFinish={handleFinish}>
        <Form.Item
          name="jobName"
          label="Job Name"
          rules={[{ required: true, message: "Please enter a job name" }]}
        >
          <Input />
        </Form.Item>

        <Form.Item
          name="projectId"
          label="Project"
          rules={[{ required: true, message: "Please select a project" }]}
        >
          <Select
            options={projects
              .filter((p) => p.id)
              .map((p) => ({ label: p.projectName ?? p.id, value: p.id }))}
          />
        </Form.Item>

        <Form.Item
          name="computeType"
          label="Compute Type"
          rules={[{ required: true, message: "Please select a compute type" }]}
        >
          <Select
            options={[
              { label: "cpu-small", value: ComputeType.CpuSmall },
              { label: "cpu-large", value: ComputeType.CpuLarge },
              { label: "gpu", value: ComputeType.Gpu },
            ]}
          />
        </Form.Item>

        <Form.Item
          name="inputFile"
          label="Input File"
          valuePropName="fileList"
          getValueFromEvent={(e) => (Array.isArray(e) ? e : e?.fileList)}
        >
          <Upload beforeUpload={() => false} maxCount={1}>
            <Button type="default">Select File</Button>
          </Upload>
        </Form.Item>

        <Form.Item>
          <Space>
            <Button onClick={onClose}>Cancel</Button>
            <Button type="primary" htmlType="submit" loading={submitting}>
              Create
            </Button>
          </Space>
        </Form.Item>
      </Form>
    </Modal>
  );
}

export default CreateJobModal;
