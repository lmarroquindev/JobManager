import { useState } from 'react';
import { Button, TextField, Box } from '@mui/material';
import { startJob } from '../../services/jobService';
import { useJobContext } from '../../context/JobContext';
import { toast } from 'react-toastify';

export default function JobForm() {
  const [jobType, setJobType] = useState('');
  const [jobName, setJobName] = useState('');
  const { refreshJobs } = useJobContext();

  const handleSubmit = async () => {
    try {
      await startJob(jobType, jobName);
      toast.success('Job started successfully.');
      await refreshJobs();
      setJobType('');
      setJobName('');
    } catch (error) {
      toast.error('Failed to start the job. Please try again.');
    }
  };

  const isFormValid = jobType.trim() !== '' && jobName.trim() !== '';

  return (
    <Box display="flex" gap={2} mb={4}>
      <TextField
        label="Job Type"
        value={jobType}
        onChange={(e) => setJobType(e.target.value)}
        required
      />
      <TextField
        label="Job Name"
        value={jobName}
        onChange={(e) => setJobName(e.target.value)}
        required
      />
      <Button
        variant="contained"
        onClick={handleSubmit}
        disabled={!isFormValid}
      >
        Start Job
      </Button>
    </Box>
  );
}
