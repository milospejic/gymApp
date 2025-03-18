import { Routes, Route } from "react-router-dom";
import Home from "../pages/Home";
import About from "../pages/About";
import Plans from "../pages/Plans";
import Login from "../pages/Login";
import Registartion from "../pages/Registration";

const AppRoutes = () => {
  return (
    <Routes>
      <Route path="/" element={<Home />} />
      <Route path="/about" element={<About />} />
      <Route path="/plans" element={<Plans />} />
      <Route path="/login" element={<Login />} />
      <Route path="/register" element={<Registartion />} />
    </Routes>
  );
};

export default AppRoutes;
