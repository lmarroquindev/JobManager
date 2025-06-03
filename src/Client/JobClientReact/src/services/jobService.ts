import api from './apiClient';
import type { Job } from '../types';

export const startJob = async (jobType: string, jobName: string) => {
  const response = await api.post('/Jobs', { jobType, jobName });
  return response.data;
};

export const getJobs = async (): Promise<Job[]> => {
  const response = await api.get('/Jobs');
  return response.data;
};

export const getJobStatus = async (id: string): Promise<string> => {
  const response = await api.get(`/Jobs/job-status/${id}`);
  return response.data.status;
};

export const cancelJob = async (jobId: string) => {
  const response = await api.post('/Jobs/cancel-job', { jobId });
  return response.data;
};
