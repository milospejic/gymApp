import { Link } from "react-router-dom";
import RegistrationForm from "../components/RegistrationForm";

const Registartion = () => {
  return (
    <div className="vh-100">
      <RegistrationForm />
      <p className="text-light text-center">
        Already have an account? <Link to="/login">Sign In</Link>
      </p>
    </div>
    
  );
};

export default Registartion;
