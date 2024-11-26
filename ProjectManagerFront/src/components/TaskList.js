import React, { useEffect, useState } from "react";
import { projectService } from "../services/projectService";
import {
  Container,
  Typography,
  List,
  ListItem,
  ListItemText,
  Checkbox,
  CircularProgress,
  Button,
  Modal,
  Box,
  Select,
  MenuItem,
  FormControl,
  InputLabel,
  TextField,
} from "@mui/material";
import { taskService } from "../services/projectTaskService";

function TaskList() {
  const [tasks, setTasks] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [openModal, setOpenModal] = useState(false);
  const [projects, setProjects] = useState([]);
  const [selectedProject, setSelectedProject] = useState('');
  const [openTaskModal, setOpenTaskModal] = useState(false);
  const [newTask, setNewTask] = useState({
    titulo: '',
    descricao: ''
  });

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

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        console.log("Buscando projetos...");
        const projectList = await projectService.getProjects();
        console.log("Projetos recebidos:", projectList);
        setProjects(projectList || []);
      } catch (err) {
        console.error("Erro ao carregar projetos:", err);
        setError("Erro ao carregar projetos. Tente novamente.");
      }
    };

    if (openModal) {
      fetchProjects();
    }
  }, [openModal]);

  const modalStyle = {
    position: 'absolute',
    top: '50%',
    left: '50%',
    transform: 'translate(-50%, -50%)',
    width: 400,
    bgcolor: 'background.paper',
    boxShadow: 24,
    p: 4,
    borderRadius: 2,
  };

  const handleToggleComplete = async (taskId, isCompleted) => {
    try {
      if (isCompleted) {
        await taskService.incompleteTask(taskId);
      } else {
        await taskService.completeTask(taskId);
      }
      
      setTasks((prevTasks) =>
        prevTasks.map((task) =>
          task.id === taskId ? { ...task, concluida: !task.concluida } : task
        )
      );
    } catch (err) {
      console.error('Error toggling task:', err);
      setError("Erro ao atualizar o status da tarefa.");
    }
  };

  const handleCreateTask = async () => {
    try {
      if (!selectedProject) {
        setError("Selecione um projeto");
        return;
      }

      const token = localStorage.getItem('token');
      const decodedToken = JSON.parse(atob(token.split('.')[1]));
      const userId = decodedToken.sub;

      const taskData = {
        titulo: newTask.titulo,
        descricao: newTask.descricao,
        projetoId: selectedProject,
        usuarioId: userId
      };

      console.log('Creating task with:', taskData);

      const result = await taskService.createTask(taskData);
      console.log('Task created:', result);

      setOpenTaskModal(false);
      setOpenModal(false);

      const userTasks = await taskService.getUserTasks();
      setTasks(userTasks || []);

      setNewTask({ titulo: '', descricao: '' });
      setSelectedProject('');
    } catch (err) {
      console.error('Error creating task:', err);
      setError(err.response?.data?.message || "Erro ao criar tarefa");
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
      <Button
        variant="contained"
        color="primary"
        sx={{ mb: 2 }}
        onClick={() => setOpenModal(true)}
      >
        Criar Tarefa
      </Button>

      <Modal
        open={openModal}
        onClose={() => setOpenModal(false)}
      >
        <Box sx={modalStyle}>
          <Typography variant="h6" component="h2" gutterBottom>
            Criar Nova Tarefa
          </Typography>
          <FormControl fullWidth sx={{ mt: 2 }}>
            <InputLabel>Selecione um Projeto</InputLabel>
            <Select
              value={selectedProject}
              onChange={(e) => setSelectedProject(e.target.value)}
              label="Selecione um Projeto"
            >
              {projects.map((project) => (
                <MenuItem key={project.id} value={project.id}>
                  {project.nome}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end', gap: 1 }}>
            <Button onClick={() => setOpenModal(false)}>Cancelar</Button>
            <Button
              variant="contained"
              color="primary"
              onClick={() => {
                setOpenModal(false);
                setOpenTaskModal(true);
              }}
              disabled={!selectedProject}
            >
              Continuar
            </Button>
          </Box>
        </Box>
      </Modal>
      <Modal
        open={openTaskModal}
        onClose={() => setOpenTaskModal(false)}
      >
        <Box sx={modalStyle}>
          <Typography variant="h6" component="h2" gutterBottom>
            Criar Nova Tarefa
          </Typography>
          <FormControl fullWidth sx={{ mt: 2 }}>
            <TextField
              label="Título"
              value={newTask.titulo}
              onChange={(e) => setNewTask({ ...newTask, titulo: e.target.value })}
              fullWidth
              sx={{ mb: 2 }}
            />
            <TextField
              label="Descrição"
              value={newTask.descricao}
              onChange={(e) => setNewTask({ ...newTask, descricao: e.target.value })}
              fullWidth
              multiline
              rows={4}
            />
          </FormControl>
          <Box sx={{ mt: 3, display: 'flex', justifyContent: 'flex-end', gap: 1 }}>
            <Button onClick={() => {
              setOpenTaskModal(false);
              setNewTask({ titulo: '', descricao: '' });
            }}>
              Cancelar
            </Button>
            <Button
              variant="contained"
              color="primary"
              onClick={handleCreateTask}
              disabled={!newTask.titulo || !newTask.descricao}
            >
              Criar Tarefa
            </Button>
          </Box>
        </Box>
      </Modal>
      {tasks.length === 0 ? (
        <Typography variant="body1" color="textSecondary" gutterBottom>
          Você ainda não possui nenhuma tarefa.
        </Typography>
      ) : (
        <List>
          {tasks.map((task) => (
            <ListItem
              key={task.id}
              sx={{
                borderBottom: '1px solid #eee',
                '&:hover': {
                  backgroundColor: '#f5f5f5'
                }
              }}
            >
              <Checkbox
                checked={task.concluida}
                onChange={() => handleToggleComplete(task.id, task.concluida)}
              />
              <ListItemText
                primary={task.titulo}
                secondary={task.descricao}
                sx={{
                  '& .MuiListItemText-primary': {
                    fontWeight: 'bold'
                  }
                }}
              />
            </ListItem>
          ))}
        </List>
      )}
    </Container>
  );
}

export default TaskList;
