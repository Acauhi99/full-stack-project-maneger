import React, { useEffect, useState } from "react";
import {
  Container,
  Typography,
  List,
  ListItem,
  ListItemText,
  Checkbox,
  CircularProgress,
} from "@mui/material";
import { taskService } from "../services/projectTaskService";

function TaskList() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchUserTasks = async () => {
      try {
        setLoading(true);
        const userTasks = await taskService.getUserTasks(); 
        setTasks(userTasks || []); 
      } catch (err) {
        setError("Erro ao carregar suas tarefas. Tente novamente.");
      } finally {
        setLoading(false);
      }
    };

    fetchUserTasks();
  }, []);

  const handleToggleComplete = async (taskId, completed) => {
    try {
      await taskService.completeTask(taskId); 
      setTasks((prevTasks) =>
        prevTasks.map((task) =>
          task.id === taskId ? { ...task, completed: !completed } : task
        )
      );
    } catch {
      setError("Erro ao marcar a tarefa como concluída.");
    }
  };

  if (loading) {
    return (
      <Container>
        <CircularProgress />
      </Container>
    );
  }

  if (error) {
    return (
      <Container>
        <Typography color="error">{error}</Typography>
      </Container>
    );
  }

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Suas Tarefas
      </Typography>

      {tasks.length === 0 ? (
        <Typography variant="body1" color="textSecondary" gutterBottom>
          Você ainda não possui nenhuma tarefa.
        </Typography>
      ) : (
        <List>
          {tasks.map((task) => (
            <ListItem key={task.id}>
              <Checkbox
                checked={task.completed}
                onChange={() => handleToggleComplete(task.id, task.completed)}
              />
              <ListItemText primary={task.title} />
            </ListItem>
          ))}
        </List>
      )}
    </Container>
  );
}

export default TaskList;
