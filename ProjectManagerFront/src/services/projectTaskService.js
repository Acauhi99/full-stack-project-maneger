import api from "./api";
import { handleApiError } from "./apiErrors";

export const taskService = {
  async getTasks() {
    try {
      const response = await api.get("/tasks");
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async getUserTasks() {
    try {
      const response = await api.get("/tasks/user");
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async getTask(id) {
    try {
      const response = await api.get(`/tasks/${id}`);
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async createTask(taskData) {
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        throw new Error('Não autenticado');
      }
      const payload = {
        titulo: taskData.titulo,
        descricao: taskData.descricao,
        projetoId: taskData.projetoId,
        usuarioId: taskData.usuarioId 
      };
      console.log('Sending task data:', payload);
      const response = await api.post("/tasks", payload);
      return response.data;
    } catch (error) {
      console.error('Create task error:', error.response?.data);
      throw error; 
    }
  },

  async updateTask(id, taskData) {
    try {
      const response = await api.put(`/tasks/${id}`, taskData);
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async deleteTask(id) {
    try {
      await api.delete(`/tasks/${id}`);
    } catch (error) {
      handleApiError(error);
    }
  },

  async completeTask(taskId) {
    try {
      await api.put("/tasks/user/complete", { taskId });
    } catch (error) {
      handleApiError(error);
    }
  },

  async incompleteTask(taskId) {
    try {
      await api.put("/tasks/user/incomplete", { taskId });
    } catch (error) {
      handleApiError(error);
    }
  },
};
