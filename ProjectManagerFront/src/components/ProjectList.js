import React from "react";
import {
  Container,
  Typography,
  List,
  ListItem,
  ListItemText,
  Button,
} from "@mui/material";

function ProjectList() {
  const projects = [
    { id: 1, name: "Projeto 1", description: "Descrição do Projeto 1" },
    { id: 2, name: "Projeto 2", description: "Descrição do Projeto 2" },
  ];

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Lista de Projetos
      </Typography>
      <List>
        {projects.map((project) => (
          <ListItem key={project.id}>
            <ListItemText
              primary={project.name}
              secondary={project.description}
            />
            <Button variant="contained" color="primary">
              Editar
            </Button>
            <Button
              variant="contained"
              color="secondary"
              style={{ marginLeft: "10px" }}
            >
              Excluir
            </Button>
          </ListItem>
        ))}
      </List>
    </Container>
  );
}

export default ProjectList;
