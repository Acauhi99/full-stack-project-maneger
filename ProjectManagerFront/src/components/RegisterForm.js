import React from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  RadioGroup,
  FormControlLabel,
  Radio,
  FormControl,
  FormLabel,
  FormHelperText,
  Link,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { userService } from "../services/userService";
import { toast } from "react-toastify";
import { Formik, Form, Field } from "formik";
import * as Yup from "yup";

const validationSchema = Yup.object().shape({
  nome: Yup.string()
    .required("Nome é obrigatório")
    .max(100, "Nome deve ter no máximo 100 caracteres"),
  email: Yup.string()
    .email("Email inválido")
    .required("Email é obrigatório")
    .max(100, "Email deve ter no máximo 100 caracteres"),
  senha: Yup.string()
    .required("Senha é obrigatória")
    .min(6, "Senha deve ter no mínimo 6 caracteres"),
  tipoUsuario: Yup.number()
    .required("Tipo de usuário é obrigatório")
    .oneOf([0, 1], "Tipo de usuário inválido"),
});

function RegisterForm() {
  const navigate = useNavigate();

  const handleSubmit = async (values, { setSubmitting }) => {
    try {
      const response = await userService.register(values);
      toast.success(response.message || "Cadastro realizado com sucesso!");
      navigate("/login");
    } catch (error) {
      if (
        error.status === 400 &&
        error.message === "Email já cadastrado no sistema"
      ) {
        toast.error("Email já está em uso");
      } else if (error.status === 400) {
        toast.error("Email e senha são obrigatórios");
      } else {
        toast.error("Erro no servidor. Tente novamente mais tarde.");
      }
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <Container maxWidth="sm">
      <Box sx={{ mt: 8 }}>
        <Typography variant="h4" gutterBottom>
          Registrar
        </Typography>
        <Formik
          initialValues={{
            nome: "",
            email: "",
            senha: "",
            tipoUsuario: "",
          }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          {({ isSubmitting, errors, touched }) => (
            <Form>
              <Field
                as={TextField}
                label="Nome"
                name="nome"
                fullWidth
                margin="normal"
                error={touched.nome && !!errors.nome}
                helperText={touched.nome && errors.nome}
              />
              <Field
                as={TextField}
                label="Email"
                name="email"
                type="email"
                fullWidth
                margin="normal"
                error={touched.email && !!errors.email}
                helperText={touched.email && errors.email}
              />
              <Field
                as={TextField}
                label="Senha"
                name="senha"
                type="password"
                fullWidth
                margin="normal"
                error={touched.senha && !!errors.senha}
                helperText={touched.senha && errors.senha}
              />
              <FormControl
                component="fieldset"
                margin="normal"
                error={touched.tipoUsuario && !!errors.tipoUsuario}
              >
                <FormLabel component="legend">Tipo de Usuário</FormLabel>
                <Field name="tipoUsuario">
                  {({ field }) => (
                    <RadioGroup {...field} row>
                      <FormControlLabel
                        value={0}
                        control={<Radio />}
                        label="Administrador"
                      />
                      <FormControlLabel
                        value={1}
                        control={<Radio />}
                        label="Regular"
                      />
                    </RadioGroup>
                  )}
                </Field>
                {touched.tipoUsuario && errors.tipoUsuario && (
                  <FormHelperText>{errors.tipoUsuario}</FormHelperText>
                )}
              </FormControl>
              <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                disabled={isSubmitting}
                sx={{ mt: 2 }}
              >
                Registrar
              </Button>
            </Form>
          )}
        </Formik>

        <Box sx={{ mt: 2 }}>
          <Typography variant="body2" align="center">
            Já possui uma conta?{" "}
            <Link href="/login" variant="body2">
              Voltar para Login
            </Link>
          </Typography>
        </Box>
      </Box>
    </Container>
  );
}

export default RegisterForm;
