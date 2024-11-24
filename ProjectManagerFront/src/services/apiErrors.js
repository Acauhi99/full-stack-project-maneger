class ApiError extends Error {
  constructor(message, status) {
    super(message);
    this.status = status;
  }
}

export const handleApiError = (error) => {
  if (error.response) {
    throw new ApiError(error.response.data.message, error.response.status);
  }
  throw new Error("Erro de conexão com o servidor");
};
