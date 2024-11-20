import React, { useState } from "react";
import { Container, TextField, Button, Typography } from "@mui/material";
import { useNavigate } from "react-router-dom";
import { register } from "../services/userService";
import { toast } from "react-toastify";

function RegisterForm() {
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await register({ name, email, password });
      toast.success("Registro bem-sucedido! Faça login.");
      navigate("/login");
    } catch (error) {
      toast.error("Erro no registro. Tente novamente.");
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Registrar
      </Typography>
      <form onSubmit={handleSubmit}>
        <TextField
          label="Nome"
          value={name}
          onChange={(e) => setName(e.target.value)}
          fullWidth
          margin="normal"
          required
        />
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
          Registrar
        </Button>
      </form>
    </Container>
  );
}

export default RegisterForm;
