import api from "./api";
import { handleApiError } from "./apiErrors";

export const projectService = {
  async getProjects() {
    try {
      const response = await api.get("/projects");
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async getProject(id) {
    try {
      const response = await api.get(`/projects/${id}`);
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async createProject(projectData) {
    try {
      const response = await api.post("/projects", {
        nome: projectData.nome,
        descricao: projectData.descricao,
      });
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async updateProject(id, projectData) {
    try {
      const response = await api.put(`/projects/${id}`, {
        nome: projectData.nome,
        descricao: projectData.descricao,
      });
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async deleteProject(id) {
    try {
      await api.delete(`/projects/${id}`);
    } catch (error) {
      handleApiError(error);
    }
  },

  async getProjectReports() {
    try {
      const response = await api.get("/projects/reports");
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },
};
