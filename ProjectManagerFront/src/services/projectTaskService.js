import api from "./api";

const getTasks = async (projectId) => {
  const response = await api.get(`/projects/${projectId}/tasks`);
  return response.data;
};

const createTask = async (projectId, taskData) => {
  const response = await api.post(`/projects/${projectId}/tasks`, taskData);
  return response.data;
};

const updateTask = async (projectId, taskId, taskData) => {
  const response = await api.put(
    `/projects/${projectId}/tasks/${taskId}`,
    taskData
  );
  return response.data;
};

const deleteTask = async (projectId, taskId) => {
  const response = await api.delete(`/projects/${projectId}/tasks/${taskId}`);
  return response.data;
};

export { getTasks, createTask, updateTask, deleteTask };
