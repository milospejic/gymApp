import axios from 'axios';

const API_BASE_URL = 'https://localhost:7151/api/admin'; // Update with your backend URL

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
  getAllAdmins: async (token: string): Promise<Admin[]> => {
    try {
      const response = await axios.get(`${API_BASE_URL}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching admins:', error);
      throw error;
    }
  },

  getMyInfo: async (token: string): Promise<Admin> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/myInfo`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching user info:', error);
      throw error;
    }
  },

  getAdminById: async (id: string, token: string): Promise<Admin> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching admin with ID ${id}:`, error);
      throw error;
    }
  },

  getAdminByEmail: async (email: string, token : string): Promise<Admin> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/email?email=${email}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching admin with email ${email}:`, error);
      throw error;
    }
  },

  createAdmin: async (adminData : AdminCreate): Promise<string> => {
    try {
      const response = await axios.post(`${API_BASE_URL}`, adminData, {
        headers: { 'Content-Type': 'application/json' },
      });
      return response.data;
    } catch (error) {
      console.error('Error creating admin:', error);
      throw error;
    }
  },

  updateAdmin: async (id: string, adminData: AdminUpdate, token: string): Promise<string> => {
    try {
      const response = await axios.put(`${API_BASE_URL}/${id}`, adminData, {
        headers: { 
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data;
    } catch (error) {
      console.error(`Error updating admin with ID ${id}:`, error);
      throw error;
    }
  },

  deleteAdmin: async (id: string, token: string): Promise<void> => {
    try {
        await axios.delete(`${API_BASE_URL}/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
          });
    } catch (error) {
      console.error(`Error deleting admin with ID ${id}:`, error);
      throw error;
    }
  },

  changeAdminPassword: async (data: PasswordUpdate, token: string): Promise<string> => {
    try {
        const response = await axios.patch(`${API_BASE_URL}/`,data, {
            headers: { Authorization: `Bearer ${token}` },
          });
          return response.data;
    } catch (error) {
        console.error(`Error changing admin password:`, error);
      throw error;
    }
  }
};

