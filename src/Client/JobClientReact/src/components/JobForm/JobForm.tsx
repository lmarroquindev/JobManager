import { useState } from 'react';
import { Button, TextField, Box } from '@mui/material';
import { startJob } from '../../services/jobService';
import { useJobContext } from '../../context/JobContext';

export default function JobForm() {
  const [jobType, setJobType] = useState('');
  const [jobName, setJobName] = useState('');
  const { refreshJobs } = useJobContext();

  const handleSubmit = async () => {
    await startJob(jobType, jobName);
    await refreshJobs(); // 👈 Actualiza sin recargar
    setJobType('');
    setJobName('');
  };

  return (
    <Box display="flex" gap={2} mb={4}>
      <TextField
        label="Job Type"
        value={jobType}
        onChange={(e) => setJobType(e.target.value)}
      />
      <TextField
        label="Job Name"
        value={jobName}
        onChange={(e) => setJobName(e.target.value)}
      />
      <Button variant="contained" onClick={handleSubmit}>
        Start Job
      </Button>
    </Box>
  );
}
