import { createContext, useContext, useState, useCallback } from 'react';
import type { Job } from '../types';
import { getJobs } from '../services/jobService';

interface JobContextType {
  jobs: Job[];
  refreshJobs: () => Promise<void>;
  setJobs: React.Dispatch<React.SetStateAction<Job[]>>;
}

const JobContext = createContext<JobContextType | undefined>(undefined);

export const JobProvider = ({ children }: { children: React.ReactNode }) => {
  const [jobs, setJobs] = useState<Job[]>([]);

  const refreshJobs = useCallback(async () => {
    const data = await getJobs();
    setJobs(data);
  }, []);

  return (
    <JobContext.Provider value={{ jobs, refreshJobs, setJobs }}>
      {children}
    </JobContext.Provider>
  );
};

export const useJobContext = () => {
  const context = useContext(JobContext);
  if (!context) throw new Error('useJobContext must be used within JobProvider');
  return context;
};
