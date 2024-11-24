import React from "react";
import {
  Container,
  TextField,
  Button,
  Typography,
  Box,
  Link,
} from "@mui/material";
import { useNavigate } from "react-router-dom";
import { userService } from "../services/userService";
import { toast } from "react-toastify";
import { Formik, Form, Field } from "formik";
import * as Yup from "yup";

const validationSchema = Yup.object().shape({
  email: Yup.string()
    .email("Email inválido")
    .required("Email é obrigatório")
    .max(100, "Email deve ter no máximo 100 caracteres"),
  senha: Yup.string()
    .required("Senha é obrigatória")
    .min(6, "Senha deve ter no mínimo 6 caracteres"),
});

function LoginForm() {
  const navigate = useNavigate();

  const handleSubmit = async (values, { setSubmitting }) => {
    try {
      const response = await userService.login(values.email, values.senha);
      toast.success(response.message || "Login realizado com sucesso!");
      navigate("/projects");
    } catch (error) {
      if (error.status === 422) {
        toast.error("Email ou senha incorretos");
      } else if (error.status === 400) {
        toast.error("Erro ao acessar banco de dados");
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
          Login
        </Typography>
        <Formik
          initialValues={{ email: "", senha: "" }}
          validationSchema={validationSchema}
          onSubmit={handleSubmit}
        >
          {({ isSubmitting, errors, touched }) => (
            <Form>
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
              <Button
                type="submit"
                variant="contained"
                color="primary"
                fullWidth
                disabled={isSubmitting}
                sx={{ mt: 2 }}
              >
                Login
              </Button>
            </Form>
          )}
        </Formik>
        <Box sx={{ mt: 2 }}>
          <Typography variant="body2">
            Não possui conta?{" "}
            <Link href="/register" variant="body2">
              Cadastre-se
            </Link>
          </Typography>
        </Box>
      </Box>
    </Container>
  );
}

export default LoginForm;
