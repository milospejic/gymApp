import apiClient from './apiClient';

const API_BASE_URL = '/api/admin'; 

export interface Admin {
    adminID: string;
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
}

export interface AdminCreate {
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
    adminHashedPassword: string;
}

export interface AdminUpdate {
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
}

export interface PasswordUpdate{
    currentPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}

export const adminService = {
  getAllAdmins: async (): Promise<Admin[]> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching admins:', error);
      throw error;
    }
  },

  getMyInfo: async (): Promise<Admin> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/myInfo`);
      return response.data;
    } catch (error) {
      console.error('Error fetching user info:', error);
      throw error;
    }
  },

  getAdminById: async (id: string): Promise<Admin> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching admin with ID ${id}:`, error);
      throw error;
    }
  },

  getAdminByEmail: async (email: string): Promise<Admin> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/email?email=${email}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching admin with email ${email}:`, error);
      throw error;
    }
  },

  createAdmin: async (adminData : AdminCreate): Promise<string> => {
    try {
      const response = await apiClient.post(`${API_BASE_URL}`, adminData);
      return response.data;
    } catch (error) {
      console.error('Error creating admin:', error);
      throw error;
    }
  },

  updateAdmin: async (id: string, adminData: AdminUpdate): Promise<string> => {
    try {
      const response = await apiClient.put(`${API_BASE_URL}/${id}`, adminData);
      return response.data;
    } catch (error) {
      console.error(`Error updating admin with ID ${id}:`, error);
      throw error;
    }
  },

  deleteAdmin: async (id: string): Promise<void> => {
    try {
        await apiClient.delete(`${API_BASE_URL}/${id}`);
    } catch (error) {
      console.error(`Error deleting admin with ID ${id}:`, error);
      throw error;
    }
  },

  changeAdminPassword: async (data: PasswordUpdate): Promise<string> => {
    try {
        const response = await apiClient.patch(`${API_BASE_URL}/`, data);
        return response.data;
    } catch (error) {
        console.error(`Error changing admin password:`, error);
      throw error;
    }
  }
};
