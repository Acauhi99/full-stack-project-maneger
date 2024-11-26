import React, { useEffect, useState } from "react";
import {
  Container,
  Typography,
  Grid,
  Card,
  CardContent,
  CardActions,
  Button,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { projectService } from "../services/projectService";

const RedButton = styled(Button)(() => ({
  backgroundColor: "red",
  color: "white",
  "&:hover": {
    backgroundColor: "darkred",
  },
}));

function ProjectList() {
  const [projects, setProjects] = useState([]);
  const [selectedProject, setSelectedProject] = useState(null);
  const [openModal, setOpenModal] = useState(false);

  useEffect(() => {
    const fetchProjects = async () => {
      const data = await projectService.getProjects();
      if (data) {
        setProjects(data);
      }
    };

    fetchProjects();
  }, []);

  const handleOpenModal = (project) => {
    setSelectedProject(project);
    setOpenModal(true);
  };

  const handleCloseModal = () => {
    setSelectedProject(null);
    setOpenModal(false);
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Lista de Projetos
      </Typography>
      {projects.length === 0 ? (
        <Typography variant="body1">
          Nenhum projeto criado ainda no sistema.
        </Typography>
      ) : (
        <Grid container spacing={2}>
          {projects.map((project) => (
            <Grid item key={project.id} xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="h5" component="div">
                    {project.nome}
                  </Typography>
                  <Typography variant="body2" color="text.secondary">
                    {project.descricao}
                  </Typography>
                </CardContent>
                <CardActions>
                  <Button size="small" onClick={() => handleOpenModal(project)}>
                    Ver Detalhes
                  </Button>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      )}

      <Dialog open={openModal} onClose={handleCloseModal} fullWidth>
        <DialogTitle>
          <Typography
            variant="h5"
            component="div"
            style={{ fontWeight: "bold" }}
          >
            {selectedProject?.nome}
          </Typography>
        </DialogTitle>
        <DialogContent dividers>
          <Typography variant="subtitle1">Descrição:</Typography>
          <Typography variant="body1" paragraph>
            {selectedProject?.descricao}
          </Typography>
        </DialogContent>
        <DialogActions>
          <RedButton onClick={handleCloseModal}>Fechar</RedButton>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

export default ProjectList;
