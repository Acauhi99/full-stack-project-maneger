import api from "./api";
import { handleApiError } from "./apiErrors";

export const userService = {
  async login(email, senha) {
    try {
      const response = await api.post("/users/login", { email, senha });
      localStorage.setItem("token", response.data.token);
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },

  async register(userData) {
    try {
      const response = await api.post("/users/register", {
        nome: userData.nome,
        email: userData.email,
        senha: userData.senha,
        tipoUsuario: userData.tipoUsuario,
      });
      return response.data;
    } catch (error) {
      handleApiError(error);
    }
  },
};
