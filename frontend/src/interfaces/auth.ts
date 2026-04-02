export interface LoginDto {
    email: string;
    password: string;
}

export interface AuthResponse {
    accessToken: string;
    refreshToken: string;
    role: string;
}

export interface AuthContextType {
  isAuthenticated: boolean;
  role: string | null;
  email: string | null;
  login: (accessToken: string, refreshToken: string, role: string) => void;
  logout: () => void;
}
