import { Route, Routes } from "react-router-dom";
import { ConfigProvider } from "antd";
import HomePage from "./pages/HomePage";
import JobDetailPage from "./pages/JobDetailPage";
import "./App.css";

function App() {
  return (
    <ConfigProvider>
      <Routes>
        <Route path="/" element={<HomePage />} />
        <Route path="/job/:jobId" element={<JobDetailPage />} />
      </Routes>
    </ConfigProvider>
  );
}

export default App;
