import LoginForm from "../components/LoginForm";
import { Link } from "react-router-dom"; // If you're using React Router

const Login = () => {
  return (
    <div className="vh-100">
      <LoginForm />
      <p className="text-light text-center ">
        Don't have an account yet? <Link to="/register">Sign Up</Link>
      </p>
    </div>
  );
};

export default Login;

