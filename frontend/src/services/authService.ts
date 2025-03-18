import axios from "axios";

const API_BASE_URL = "https://localhost:7151/api/auth";

interface LoginDto {
  email: string;
  password: string;
}

interface AuthResponse {
  token: string;
  role: string;
}

export const authService = {
  login: async (loginDto: LoginDto): Promise<AuthResponse> => {
    try {
      const response = await axios.post<AuthResponse>(`${API_BASE_URL}/login`, loginDto);


      return response.data;
    } catch (error) {
      throw new Error("Login failed. Please try again. Error: " + error);
    }
  },

  logout: () => {
    localStorage.removeItem("token");
  },

  getToken: () => {
    return localStorage.getItem("token");
  },

  isAuthenticated: () => {
    return !!localStorage.getItem("token");
  }
};
