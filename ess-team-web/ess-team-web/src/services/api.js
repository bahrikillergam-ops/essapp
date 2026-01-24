import axios from 'axios';

const API_URL = 'http://localhost:5164/api'; 
const api = axios.create({ baseURL: API_URL });

export const playerService = {
    getAll: () => api.get('/players'),
    create: (data) => api.post('/players', data),
    delete: (id) => api.delete(`/players/${id}`),
    update: (id, data) => api.put(`/players/${id}`, data)
};

export const matchService = {
    getAll: () => api.get('/matches'),
    create: (data) => api.post('/matches', data),
    delete: (id) => api.delete(`/matches/${id}`)
};

export const equipmentService = {
    getAll: () => api.get('/equipment'),
    updateQuantity: (id, qty) => api.patch(`/equipment/${id}`, { quantity: qty })
};

// Add similar services for Trainings andg Managers...
export const trainingService = {
    getAll: () => api.get('/trainings'),
    create: (data) => api.post('/trainings', data),
    delete: (id) => api.delete(`/trainings/${id}`)
};

export const managerService = {
    getAll: () => api.get('/managers'),
    create: (data) => api.post('/managers', data),
    delete: (id) => api.delete(`/managers/${id}`)
};

export const genericService = (endpoint) => ({
    getAll: () => api.get(`/${endpoint}`),
    // This sends the new data to your .NET Controller
    create: (data) => api.post(`/${endpoint}`, data), 
    delete: (id) => api.delete(`/${endpoint}/${id}`)
});