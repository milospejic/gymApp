import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import Navbar from "./components/Navbar";
import AppRoutes from "./routes/AppRoutes";

const App = () => {
  return (
    <div className='d-flex flex-column w-100 bg-dark'>
      <Navbar /> 
        <AppRoutes />
    </div>
  );
};

export default App;
