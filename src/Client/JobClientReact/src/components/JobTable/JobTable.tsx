import { useEffect } from 'react';
import { cancelJob } from '../../services/jobService';
import { useJobContext } from '../../context/JobContext';
import { useJobWebSocket } from '../../hooks/useJobWebSocket';
import { JobStatus } from '../../types';

import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableRow,
  Button,
} from '@mui/material';

export default function JobTable() {
  const { jobs, refreshJobs } = useJobContext();

  useEffect(() => {
    refreshJobs();
  }, [refreshJobs]);

  const handleCancel = async (id: string) => {
    await cancelJob(id);
    refreshJobs();
  };

  const handleJobEvent = (event: any) => {
    if (['JobStarted', 'JobCompleted', 'JobCancelled'].includes(event.eventType)) {
      refreshJobs();
    }
  };

  useJobWebSocket(`${import.meta.env.VITE_WS_URL}`, handleJobEvent);

  return (
    <Table>
      <TableHead>
        <TableRow>
          <TableCell>Job ID</TableCell>
          <TableCell>Type</TableCell>
          <TableCell>Name</TableCell>
          <TableCell>Status</TableCell>
          <TableCell>Actions</TableCell>
        </TableRow>
      </TableHead>
      <TableBody>
        {jobs.map((job) => (
          <TableRow key={job.id}>
            <TableCell>{job.id}</TableCell>
            <TableCell>{job.jobType}</TableCell>
            <TableCell>{job.jobName}</TableCell>
            <TableCell>{job.status}</TableCell>
            <TableCell>
              {
                job.status === JobStatus.Running && (
                <Button
                  variant="outlined"
                  color="error"
                  onClick={() => handleCancel(job.id)}
                >
                  Cancel
                </Button>
                )
              }
             
            </TableCell>
          </TableRow>
        ))}
      </TableBody>
    </Table>
  );
}
