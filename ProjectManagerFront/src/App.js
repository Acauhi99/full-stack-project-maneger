import React from "react";
import {
  BrowserRouter as Router,
  Route,
  Routes,
  Navigate,
} from "react-router-dom";
import LoginForm from "./components/LoginForm";
import ProjectList from "./components/ProjectList";
import TaskList from "./components/TaskList";
import AdminPanel from "./components/AdminPanel";
import HomePage from "./components/HomePage";
import RegisterForm from "./components/RegisterForm";
import { ToastContainer } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";
import Header from "./components/Header";
import { jwtDecode } from "jwt-decode";
import { Box } from "@mui/material";

function PrivateRoute({ children }) {
  const token = localStorage.getItem("token");
  const isAuthenticated = token && jwtDecode(token).exp > Date.now() / 1000;

  return isAuthenticated ? children : <Navigate to="/login" />;
}

function App() {
  return (
    <Router>
      <div className="App">
        <ToastContainer
          position="bottom-center"
          autoClose={3600}
          style={{ width: "400px", textAlign: "center" }}
        />
        <Header />
        <Box sx={{ marginTop: 10 }}>
          <Routes>
            <Route path="/login" element={<LoginForm />} />
            <Route path="/register" element={<RegisterForm />} />
            <Route path="/" element={<HomePage />} />
            <Route
              path="/projects"
              element={
                <PrivateRoute>
                  <ProjectList />
                </PrivateRoute>
              }
            />
            <Route
              path="/tasks"
              element={
                <PrivateRoute>
                  <TaskList />
                </PrivateRoute>
              }
            />
            <Route
              path="/admin"
              element={
                <PrivateRoute>
                  <AdminPanel />
                </PrivateRoute>
              }
            />
          </Routes>
        </Box>
      </div>
    </Router>
  );
}

export default App;
