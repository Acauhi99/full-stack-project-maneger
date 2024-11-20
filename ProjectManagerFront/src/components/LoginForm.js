import React, { useState } from "react";
import { Container, TextField, Button, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { login } from "../services/userService";
import { toast } from "react-toastify";

function LoginForm() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await login(email, password);
      localStorage.setItem("token", response.token);
      toast.success("Login bem-sucedido!");
      navigate("/projects");
    } catch (error) {
      toast.error("Erro no login. Tente novamente.");
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Login
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Email"
          type="email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <TextField
          label="Senha"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
        <Button type="submit" variant="contained" color="primary">
          Login
        </Button>
      </form>
    </Container>
  );
}

export default LoginForm;
