import { Routes, Route, Navigate } from "react-router-dom"; 
import { JSX, useContext } from "react"; 
import { AuthContext } from "../contexts/AuthContext"; 

import Home from "../pages/Home";
import About from "../pages/About";
import Plans from "../pages/Plans";
import Login from "../pages/Login";
import Registration from "../pages/Registration"; 

import { Profile } from "../pages/Profile"; 
import { MembershipPage } from "../pages/Membership";

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
  const authContext = useContext(AuthContext);
  
  if (!authContext?.isAuthenticated) {
    return <Navigate to="/login" replace />;
  }
  
  return children;
};

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      <Route path="/plans" element={<Plans />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Registration />} />
      
      <Route 
        path="/profile" 
        element={
          <ProtectedRoute>
            <Profile />
          </ProtectedRoute>
        } 
      />
      <Route 
        path="/membership" 
        element={
          <ProtectedRoute>
            <MembershipPage />
          </ProtectedRoute>
        }
      />
    </Routes>
  );
};

export default AppRoutes;