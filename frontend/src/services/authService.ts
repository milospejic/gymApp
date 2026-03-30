import apiClient from "./apiClient";

const API_BASE_URL = "/api/auth";

interface LoginDto {
  email: string;
  password: string;
}

interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  role: string;
}

export const authService = {
  login: async (loginDto: LoginDto): Promise<AuthResponse> => {
    try {
      const response = await apiClient.post<AuthResponse>(`${API_BASE_URL}/login`, loginDto);
      return response.data;
    } catch (error) {
      throw new Error("Login failed. Please try again. Error: " + error);
    }
  },

  logout: () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
    localStorage.removeItem("role");
  },

  getToken: () => {
    return localStorage.getItem("accessToken");
  },

  isAuthenticated: () => {
    return !!localStorage.getItem("accessToken");
  }
};
