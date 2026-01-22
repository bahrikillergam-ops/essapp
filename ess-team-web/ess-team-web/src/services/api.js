import axios from 'axios';

// Replace 5xxx with your .NET port number
const API_URL = 'http://localhost:5164/api'; 

const api = axios.create({
  baseURL: API_URL,
});

export const managerService = {
  // This function "calls" the backend to get the manager list
  getAll: () => api.get('/managers'),
};

export default api;