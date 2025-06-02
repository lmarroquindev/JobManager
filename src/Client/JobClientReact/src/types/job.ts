export interface Job {
  id: string;
  jobType: string;
  jobName: string;
  status: 'Running' | 'Not Running';
}
