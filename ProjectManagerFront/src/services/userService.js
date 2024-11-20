import api from "./api";

const login = async (email, password) => {
  const response = await api.post("/users/login", { email, password });
  return response.data;
};

const register = async (userData) => {
  const response = await api.post("/users/register", userData);
  return response.data;
};

export { login, register };
