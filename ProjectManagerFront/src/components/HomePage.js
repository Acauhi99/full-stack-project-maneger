import React from "react";
import { Container, Typography, Button, Box, Paper } from "@mui/material";
import { Link } from "react-router-dom";

function HomePage() {
  return (
    <Container maxWidth="md">
      <Box sx={{ mt: 8 }}>
        <Paper elevation={3} sx={{ p: 4 }}>
          <Typography variant="h4" gutterBottom>
            Bem-vindo ao Sistema de Gerenciamento de Projetos
          </Typography>
          <Typography variant="body1" paragraph sx={{ fontSize: "1.2rem" }}>
            O Sistema de Gerenciamento de Projetos permite que administradores
            criem, editem e excluam projetos, além de atribuir tarefas a
            usuários regulares. Os usuários regulares podem visualizar suas
            tarefas e marcar tarefas como concluídas.
          </Typography>
          <Typography variant="h6" gutterBottom>
            Funcionalidades:
          </Typography>
          <Typography variant="body1" paragraph sx={{ fontSize: "1.2rem" }}>
            <strong>Autenticação e Autorização:</strong>
            <ul>
              <li>Usuários logam no sistema utilizando email e senha.</li>
              <li>Dois níveis de acesso: Admin e Usuário Regular.</li>
            </ul>
            <strong>CRUD para Projetos e Tarefas:</strong>
            <ul>
              <li>Projeto: Criar, editar, listar e excluir projetos.</li>
              <li>
                Tarefa: Criar, editar, listar e excluir tarefas associadas a um
                projeto.
              </li>
              <li>Marcar tarefas como concluídas.</li>
            </ul>
            <strong>Relatórios (Opcional):</strong>
            <ul>
              <li>
                Administradores podem gerar relatórios sobre a quantidade de
                tarefas por projeto ou usuário.
              </li>
            </ul>
          </Typography>
          <Box sx={{ mt: 4 }}>
            <Button
              variant="contained"
              color="primary"
              component={Link}
              to="/login"
            >
              Login
            </Button>
            <Button
              variant="contained"
              color="secondary"
              component={Link}
              to="/register"
              sx={{ ml: 2 }}
            >
              Registrar
            </Button>
          </Box>
        </Paper>
      </Box>
    </Container>
  );
}

export default HomePage;
