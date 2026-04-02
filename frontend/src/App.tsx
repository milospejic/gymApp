import Navbar from "./components/Navbar";
import AppRoutes from "./routes/AppRoutes";
import { AuthProvider } from "./contexts/AuthContext";
import "./App.css";

const App = () => {
  return (
    <AuthProvider>
      <div className='app-container'>
        <Navbar />
        <main className="main-content">
          <AppRoutes />
        </main>
      </div>
    </AuthProvider>
  );
};

export default App;