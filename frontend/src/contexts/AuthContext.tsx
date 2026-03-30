import { createContext, useContext, useEffect, useState, ReactNode } from "react";

interface AuthContextType {
  isAuthenticated: boolean;
  role: string | null;
  login: (accessToken: string, refreshToken: string, role: string) => void;
  logout: () => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!localStorage.getItem("accessToken"));
  const [role, setRole] = useState<string | null>(localStorage.getItem("role"));

  useEffect(() => {
    const accessToken = localStorage.getItem("accessToken");
    const userRole = localStorage.getItem("role");

    if (accessToken) {
      setIsAuthenticated(true);
      setRole(userRole);
    }
  }, []);

  const login = (accessToken: string, refreshToken: string, role: string) => {
    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
    localStorage.setItem("role", role);
    setIsAuthenticated(true);
    setRole(role);
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("role");
    setIsAuthenticated(false);
    setRole(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
