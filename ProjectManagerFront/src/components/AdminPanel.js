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
  TextField,
  Box,
} from "@mui/material";
import { styled } from "@mui/material/styles";
import { projectService } from "../services/projectService";
import { toast } from "react-toastify";

const RedButton = styled(Button)(() => ({
  backgroundColor: "#f44336",
  color: "white",
  "&:hover": {
    backgroundColor: "#d32f2f",
  },
}));

const GreenButton = styled(Button)(() => ({
  backgroundColor: "#4caf50",
  color: "white",
  "&:hover": {
    backgroundColor: "#388e3c",
  },
}));

function AdminPanel() {
  const [projects, setProjects] = useState([]);
  const [selectedProject, setSelectedProject] = useState(null);
  const [openViewModal, setOpenViewModal] = useState(false);
  const [openEditModal, setOpenEditModal] = useState(false);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);
  const [openCreateModal, setOpenCreateModal] = useState(false);
  const [formValues, setFormValues] = useState({
    nome: "",
    descricao: "",
  });
  const [reports, setReports] = useState([]);

  useEffect(() => {
    fetchProjects();
    fetchReports();
  }, []);

  const fetchProjects = async () => {
    try {
      const data = await projectService.getProjects();
      if (data) {
        setProjects(data);
      }
    } catch (error) {
      toast.error("Erro ao buscar projetos.");
    }
  };

  const fetchReports = async () => {
    try {
      const data = await projectService.getProjectReports();
      if (data) {
        setReports(data);
      }
    } catch (error) {
      toast.error("Erro ao buscar relatórios.");
    }
  };

  const handleOpenViewModal = (project) => {
    setSelectedProject(project);
    setOpenViewModal(true);
  };

  const handleCloseViewModal = () => {
    setSelectedProject(null);
    setOpenViewModal(false);
  };

  const handleOpenEditModal = (project) => {
    setSelectedProject(project);
    setFormValues({
      nome: project.nome,
      descricao: project.descricao,
    });
    setOpenEditModal(true);
  };

  const handleCloseEditModal = () => {
    setSelectedProject(null);
    setFormValues({ nome: "", descricao: "" });
    setOpenEditModal(false);
  };

  const handleOpenDeleteDialog = (project) => {
    setSelectedProject(project);
    setOpenDeleteDialog(true);
  };

  const handleCloseDeleteDialog = () => {
    setSelectedProject(null);
    setOpenDeleteDialog(false);
  };

  const handleOpenCreateModal = () => {
    setFormValues({ nome: "", descricao: "" });
    setOpenCreateModal(true);
  };

  const handleCloseCreateModal = () => {
    setFormValues({ nome: "", descricao: "" });
    setOpenCreateModal(false);
  };

  const handleCreateProject = async () => {
    try {
      await projectService.createProject(formValues);
      toast.success("Projeto criado com sucesso.");
      fetchProjects();
      fetchReports();
      handleCloseCreateModal();
    } catch (error) {
      toast.error("Erro ao criar projeto.");
    }
  };

  const handleEditProject = async () => {
    try {
      await projectService.updateProject(selectedProject.id, formValues);
      toast.success("Projeto atualizado com sucesso.");
      fetchProjects();
      fetchReports();
      handleCloseEditModal();
    } catch (error) {
      toast.error("Erro ao atualizar projeto.");
    }
  };

  const handleDeleteProject = async () => {
    try {
      await projectService.deleteProject(selectedProject.id);
      toast.success("Projeto deletado com sucesso.");
      fetchProjects();
      fetchReports();
      handleCloseDeleteDialog();
    } catch (error) {
      toast.error("Erro ao deletar projeto.");
    }
  };

  const handleChange = (e) => {
    setFormValues({ ...formValues, [e.target.name]: e.target.value });
  };

  return (
    <Container>
      <Typography variant="h4" gutterBottom>
        Painel Administrativo
      </Typography>
      <GreenButton onClick={handleOpenCreateModal}>Criar Projeto</GreenButton>
      {/* Projects List */}
      <Box sx={{ mt: 4 }}>
        <Typography variant="h5">Projetos</Typography>
        <Grid container spacing={2}>
          {projects.map((project) => (
            <Grid item key={project.id} xs={12} sm={6} md={4}>
              <Card>
                <CardContent>
                  <Typography variant="h6">{project.nome}</Typography>
                  <Typography variant="body2">{project.descricao}</Typography>
                </CardContent>
                <CardActions>
                  <Button onClick={() => handleOpenViewModal(project)}>
                    Ver
                  </Button>
                  <Button onClick={() => handleOpenEditModal(project)}>
                    Editar
                  </Button>
                  <RedButton onClick={() => handleOpenDeleteDialog(project)}>
                    Deletar
                  </RedButton>
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      </Box>
      {/* Reports Section */}
      <Box sx={{ mt: 4 }}>
        <Typography variant="h5">Relatórios de Projetos</Typography>
        {reports.length === 0 ? (
          <Typography variant="body1">Nenhum relatório disponível.</Typography>
        ) : (
          <Grid container spacing={2}>
            {reports.map((report) => (
              <Grid item key={report.projectId} xs={12} sm={6} md={4}>
                <Card>
                  <CardContent>
                    <Typography variant="h6">{report.projectName}</Typography>
                    <Typography variant="body2">
                      Tarefas: {report.taskCount}
                    </Typography>
                  </CardContent>
                </Card>
              </Grid>
            ))}
          </Grid>
        )}
      </Box>
      {/* Modals and Dialogs */}
      {/* View Modal */}
      <Dialog open={openViewModal} onClose={handleCloseViewModal}>
        <DialogTitle>Detalhes do Projeto</DialogTitle>
        <DialogContent>
          {selectedProject && (
            <>
              <Typography variant="h6">{selectedProject.nome}</Typography>
              <Typography variant="body1">
                {selectedProject.descricao}
              </Typography>
            </>
          )}
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseViewModal}>Fechar</Button>
        </DialogActions>
      </Dialog>
      {/* Edit Modal */}
      <Dialog open={openEditModal} onClose={handleCloseEditModal}>
        <DialogTitle>Editar Projeto</DialogTitle>
        <DialogContent>
          <TextField
            margin="dense"
            label="Nome"
            name="nome"
            fullWidth
            value={formValues.nome}
            onChange={handleChange}
          />
          <TextField
            margin="dense"
            label="Descrição"
            name="descricao"
            fullWidth
            multiline
            rows={4}
            value={formValues.descricao}
            onChange={handleChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseEditModal}>Cancelar</Button>
          <Button onClick={handleEditProject} color="primary">
            Salvar
          </Button>
        </DialogActions>
      </Dialog>
      {/* Delete Dialog */}
      <Dialog open={openDeleteDialog} onClose={handleCloseDeleteDialog}>
        <DialogTitle>Confirmar Deleção</DialogTitle>
        <DialogContent>
          <Typography>
            Tem certeza que deseja deletar o projeto "{selectedProject?.nome}"?
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog}>Cancelar</Button>
          <RedButton onClick={handleDeleteProject}>Deletar</RedButton>
        </DialogActions>
      </Dialog>
      {/* Create Modal */}
      <Dialog open={openCreateModal} onClose={handleCloseCreateModal}>
        <DialogTitle>Criar Novo Projeto</DialogTitle>
        <DialogContent>
          <TextField
            margin="dense"
            label="Nome"
            name="nome"
            fullWidth
            value={formValues.nome}
            onChange={handleChange}
          />
          <TextField
            margin="dense"
            label="Descrição"
            name="descricao"
            fullWidth
            multiline
            rows={4}
            value={formValues.descricao}
            onChange={handleChange}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseCreateModal}>Cancelar</Button>
          <GreenButton onClick={handleCreateProject}>Criar</GreenButton>
        </DialogActions>
      </Dialog>
    </Container>
  );
}

export default AdminPanel;
