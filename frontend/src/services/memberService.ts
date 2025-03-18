import axios from 'axios';

const API_BASE_URL = 'https://localhost:7151/api/member'; // Update with your backend URL

export interface Member {
    memberID: string;
    memberName: string;
    memberSurname: string;
    memberEmail: string; 
    memberPhone: string; 
    membershipId: string;  
}

export interface MembershipCreate {
    planDuration: number;
    membershipPlanId: string; 
}

export interface MemberCreate {
    memberName: string;
    memberSurname: string;
    memberEmail: string;
    memberPhone: string;
    memberHashedPassword: string;
    membership: MembershipCreate;
}

export interface MemberUpdate {
    memberName: string;
    memberSurname: string;
    memberEmail: string;
    memberPhone: string;
}

export interface PasswordUpdate{
    currentPassword: string;
    newPassword: string;
    confirmNewPassword: string;
}

export const memberService = {
  getAllMembers: async (token: string): Promise<Member[]> => {
    try {
      const response = await axios.get(`${API_BASE_URL}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error('Error fetching members:', error);
      throw error;
    }
  },

  getMyInfo: async (token: string): Promise<Member> => {
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

  getMemberById: async (id: string, token: string): Promise<Member> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with ID ${id}:`, error);
      throw error;
    }
  },

  getMemberByMembershipId: async (membershipId: string, token: string): Promise<Member> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/membership?membershipId=${membershipId}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with Membership ID ${membershipId}:`, error);
      throw error;
    }
  },

  getMemberByEmail: async (email: string, token : string): Promise<Member> => {
    try {
      const response = await axios.get(`${API_BASE_URL}/email?email=${email}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with email ${email}:`, error);
      throw error;
    }
  },

  createMember: async (memberData : MemberCreate): Promise<string> => {
    try {
      const response = await axios.post(`${API_BASE_URL}`, memberData, {
        headers: { 'Content-Type': 'application/json' },
      });
      return response.data;
    } catch (error) {
      console.error('Error creating member:', error);
      throw error;
    }
  },

  updateMember: async (id: string, memberData: MemberUpdate, token: string): Promise<string> => {
    try {
      const response = await axios.put(`${API_BASE_URL}/${id}`, memberData, {
        headers: { 
          'Content-Type': 'application/json',
          Authorization: `Bearer ${token}`,
        },
      });
      return response.data;
    } catch (error) {
      console.error(`Error updating member with ID ${id}:`, error);
      throw error;
    }
  },

  deleteMember: async (id: string, token: string): Promise<void> => {
    try {
        await axios.delete(`${API_BASE_URL}/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
          });
    } catch (error) {
      console.error(`Error deleting member with ID ${id}:`, error);
      throw error;
    }
  },

  changeMemberPassword: async (data: PasswordUpdate, token: string): Promise<string> => {
    try {
        const response = await axios.patch(`${API_BASE_URL}/`,data, {
            headers: { Authorization: `Bearer ${token}` },
          });
        return response.data;
    } catch (error) {
        console.error(`Error changing member password:`, error);
      throw error;
    }
  }
};

