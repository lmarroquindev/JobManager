import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';
import './index.css';
import { JobProvider } from './context/JobContext';

import { ErrorBoundary } from './components/ErrorBoundary';
  import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <ErrorBoundary>
      <JobProvider>
        <App />
        <ToastContainer position="top-right" autoClose={5000} />
      </JobProvider>
  </ErrorBoundary>
  </React.StrictMode>
);
