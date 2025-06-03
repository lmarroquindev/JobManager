export interface Job {
  id: string;
  jobType: string;
  jobName: string;
  status: JobStatus
}


export const JobStatus = {
  Running: 'Running',
  NotRunning: 'Not Running',
} as const;

export type JobStatus = keyof typeof JobStatus

