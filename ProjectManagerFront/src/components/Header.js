import React from "react";
import { AppBar, Toolbar, Typography, Button, Box } from "@mui/material";
import { Link, useLocation } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

function Header() {
  const location = useLocation();
  const token = localStorage.getItem("token");
  let isAdmin = false;

  if (token) {
    const decodedToken = jwtDecode(token);
    isAdmin =
      decodedToken[
        "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
      ] === "Admin";
  }

  if (
    (location.pathname === "/") |
    (location.pathname === "/login") |
    (location.pathname === "/register")
  ) {
    return null;
  }

  return (
    <AppBar position="static">
      <Toolbar>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          Sistema de Gerenciamento de Projetos
        </Typography>
        <Box>
          <Button color="inherit" component={Link} to="/projects">
            Projetos
          </Button>
          <Button color="inherit" component={Link} to="/tasks">
            Tarefas
          </Button>
          {isAdmin && (
            <Button color="inherit" component={Link} to="/admin">
              Admin Panel
            </Button>
          )}
          <Button color="inherit" component={Link} to="/login">
            Sair
          </Button>
        </Box>
      </Toolbar>
    </AppBar>
  );
}

export default Header;
