import React, { useState, useEffect } from "react";
import {
  Container,
  Typography,
  List,
  ListItem,
  ListItemText,
  Button,
  IconButton,
  Collapse,
  Box,
} from "@mui/material";
import { ExpandLess, ExpandMore } from "@mui/icons-material";
import { projectService } from "../services/projectService";

function AdminPanel() {
  const [projects, setProjects] = useState([]);
  const [expandedProject, setExpandedProject] = useState(null); 
  const [tasks, setTasks] = useState([]); 
  const [stats, setStats] = useState(null); 

  useEffect(() => {
    fetchProjects();
    fetchProjectStats();
  }, []);

  const fetchProjects = async () => {
    const projectList = await projectService.getProjects();
    setProjects(projectList || []);
  };

  const fetchProjectStats = async () => {
    const statistics = await projectService.getProjectReports();
    setStats(statistics || {});
  };

  const fetchTasks = async (projectId) => {
    try {
      const response = await projectService.getTasks(projectId);
      setTasks(response || []);
    } catch (error) {
      console.error("Erro ao buscar tarefas:", error);
    }
  };

  const handleExpand = (projectId) => {
    if (expandedProject === projectId) {
      setExpandedProject(null);
      setTasks([]);
    } else {
      setExpandedProject(projectId);
      fetchTasks(projectId); 
    }
  };

  const handleEditProject = async (projectId) => {
    const newName = prompt("Digite o novo nome do projeto:");
    const newDescription = prompt("Digite a nova descrição do projeto:");
    if (newName && newDescription) {
      await projectService.updateProject(projectId, {
        nome: newName,
        descricao: newDescription,
      });
      fetchProjects(); 
    }
  };

  const handleDeleteProject = async (projectId) => {
    if (window.confirm("Tem certeza que deseja excluir este projeto?")) {
      await projectService.deleteProject(projectId);
      fetchProjects(); 
    }
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Painel do Administrador
      </Typography>

      {stats && (
        <Box mb={4}>
          <Typography variant="h6">Estatísticas dos Projetos</Typography>
          <Typography>Projetos Ativos: {stats.activeProjects || 0}</Typography>
          <Typography>Total de Tarefas: {stats.totalTasks || 0}</Typography>
        </Box>
      )}

      <Typography variant="h5" gutterBottom>
        Projetos
      </Typography>
      <List>
        {projects.map((project) => (
          <React.Fragment key={project.id}>
            <ListItem>
              <ListItemText
                primary={project.nome}
                secondary={project.descricao}
              />
              <IconButton onClick={() => handleExpand(project.id)}>
                {expandedProject === project.id ? <ExpandLess /> : <ExpandMore />}
              </IconButton>
              <Button
                variant="contained"
                color="primary"
                onClick={() => handleEditProject(project.id)}
              >
                Editar
              </Button>
              <Button
                variant="contained"
                color="secondary"
                onClick={() => handleDeleteProject(project.id)}
                style={{ marginLeft: "10px" }}
              >
                Excluir
              </Button>
            </ListItem>

            <Collapse in={expandedProject === project.id} timeout="auto" unmountOnExit>
              <Box pl={4}>
                <Typography variant="subtitle1" gutterBottom>
                  Tarefas do Projeto
                </Typography>
                <List>
                  {tasks.length > 0 ? (
                    tasks.map((task) => (
                      <ListItem key={task.id}>
                        <ListItemText
                          primary={task.nome}
                          secondary={task.descricao}
                        />
                      </ListItem>
                    ))
                  ) : (
                    <Typography>Nenhuma tarefa encontrada.</Typography>
                  )}
                </List>
              </Box>
            </Collapse>
          </React.Fragment>
        ))}
      </List>
    </Container>
  );
}

export default AdminPanel;
