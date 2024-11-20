import React from "react";
import { Container, Typography, Button } from "@mui/material";
import { Link } from "react-router-dom";

function HomePage() {
  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Bem-vindo ao Sistema de Gerenciamento de Projetos
      </Typography>
      <Button variant="contained" color="primary" component={Link} to="/login">
        Login
      </Button>
      <Button
        variant="contained"
        color="secondary"
        component={Link}
        to="/register"
        style={{ marginLeft: "10px" }}
      >
        Registrar
      </Button>
    </Container>
  );
}

export default HomePage;
