import React from "react";
import { Container, Typography, Button } from "@mui/material";

function AdminPanel() {
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Painel do Administrador
      </Typography>
      <Button variant="contained" color="primary">
        Gerenciar Usuários
      </Button>
      <Button
        variant="contained"
        color="secondary"
        style={{ marginLeft: "10px" }}
      >
        Gerenciar Projetos
      </Button>
    </Container>
  );
}

export default AdminPanel;
