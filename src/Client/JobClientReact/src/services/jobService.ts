import axios from 'axios';
import type { Job } from '../types';

const API_BASE = 'http://localhost:5292/api/Jobs';

export const startJob = async (jobType: string, jobName: string) => {
  const response = await axios.post(`${API_BASE}`, { jobType, jobName });
  return response.data;
};

export const getJobs = async (): Promise<Job[]> => {
  const response = await axios.get(`${API_BASE}`);
  return response.data;
};

export const getJobStatus = async (id: string): Promise<string> => {
  const response = await axios.get(`${API_BASE}/job-status/${id}`);
  return response.data.status;
};

export const cancelJob = async (id: string) => {
  const response = await axios.post(`${API_BASE}/cancel-job`, { id });
  return response.data;
};
