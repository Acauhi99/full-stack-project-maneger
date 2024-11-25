import api from "./api";
import { handleApiError } from "./apiErrors";

export const parseJwt = (token) => {
  try {
    return JSON.parse(atob(token.split('.')[1]));
  } catch (e) {
    return null;
  }
};
export const userService = {

  async login(email, senha) {
    try {
      const response = await api.post("/users/login", { email, senha });
      console.log(response.data.token);
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
