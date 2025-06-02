import { Container, Typography } from '@mui/material';
import JobForm from './components/JobForm/JobForm';
import JobTable from './components/JobTable/JobTable';

function App() {
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Job Manager
      </Typography>
      <JobForm />
      <JobTable />
    </Container>
  );
}

export default App;
