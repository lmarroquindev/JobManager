import axios from 'axios';
import { toast } from 'react-toastify';

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

api.interceptors.response.use(
  (response) => {
    const message = response.data?.message || response.data?.Message;
    if (message) {
      toast.success(message);
    }
    return response;
  },
  (error) => {
    const errorMsg =
      error.response?.data?.error ||
      error.response?.data?.message ||
      'An unexpected error occurred.';
    toast.error(errorMsg);
    return Promise.reject(error);
  }
);

export default api;
