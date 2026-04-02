import { createContext, useEffect, useState, ReactNode, useContext } from "react";
import { jwtDecode, JwtPayload } from "jwt-decode";
import { AuthContextType } from "../interfaces";

interface CustomJwtPayload extends JwtPayload {
  "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"?: string;
  email?: string;
}

export const AuthContext = createContext<AuthContextType | null>(null);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(!!localStorage.getItem("accessToken"));
  const [role, setRole] = useState<string | null>(localStorage.getItem("role"));
  const [email, setEmail] = useState<string | null>(localStorage.getItem("email"));

  useEffect(() => {
    const token = localStorage.getItem("accessToken");
    const userRole = localStorage.getItem("role");
    const userEmail = localStorage.getItem("email");

    if (token) {
      setIsAuthenticated(true);
      setRole(userRole);
      setEmail(userEmail);
    }
  }, []);

  const login = (accessToken: string, refreshToken: string, role: string) => {
    // Decode the JWT to get the user's email
    const decodedToken = jwtDecode<CustomJwtPayload>(accessToken);
    // C# stores the email under this specific schema URL in the token
    const userEmail = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"] || decodedToken.email || "";

    localStorage.setItem("accessToken", accessToken);
    localStorage.setItem("refreshToken", refreshToken);
    localStorage.setItem("role", role);
    localStorage.setItem("email", userEmail);
    
    setIsAuthenticated(true);
    setRole(role);
    setEmail(userEmail);
  };

  const logout = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("role");
    localStorage.removeItem("email");
    
    setIsAuthenticated(false);
    setRole(null);
    setEmail(null);
  };

  return (
    <AuthContext.Provider value={{ isAuthenticated, role, email, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};