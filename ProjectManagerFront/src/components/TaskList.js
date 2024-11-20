import React from "react";
import {
  Container,
  Typography,
  List,
  ListItem,
  ListItemText,
  Checkbox,
} from "@mui/material";

function TaskList() {
  const tasks = [
    { id: 1, title: "Tarefa 1", completed: false },
    { id: 2, title: "Tarefa 2", completed: true },
  ];

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Lista de Tarefas
      </Typography>
      <List>
        {tasks.map((task) => (
          <ListItem key={task.id}>
            <Checkbox checked={task.completed} />
            <ListItemText primary={task.title} />
          </ListItem>
        ))}
      </List>
    </Container>
  );
}

export default TaskList;
