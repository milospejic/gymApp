import apiClient from './apiClient';
import { Member, MemberCreate, MemberUpdate, PasswordUpdate } from '../interfaces';

const API_BASE_URL = '/api/member';

export const memberService = {
  getAllMembers: async (): Promise<Member[]> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}`);
      return response.data;
    } catch (error) {
      console.error('Error fetching members:', error);
      throw error;
    }
  },

  getMyInfo: async (): Promise<Member> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/myInfo`);
      return response.data;
    } catch (error) {
      console.error('Error fetching user info:', error);
      throw error;
    }
  },

  getMemberById: async (id: string): Promise<Member> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with ID ${id}:`, error);
      throw error;
    }
  },

  getMemberByMembershipId: async (membershipId: string): Promise<Member> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/membership?membershipId=${membershipId}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with Membership ID ${membershipId}:`, error);
      throw error;
    }
  },

  getMemberByEmail: async (email: string): Promise<Member> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/email?email=${email}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching member with email ${email}:`, error);
      throw error;
    }
  },

  createMember: async (memberData : MemberCreate): Promise<string> => {
    try {
      const response = await apiClient.post(`${API_BASE_URL}`, memberData);
      return response.data;
    } catch (error) {
      console.error('Error creating member:', error);
      throw error;
    }
  },

  updateMember: async (id: string, memberData: MemberUpdate): Promise<string> => {
    try {
      const response = await apiClient.put(`${API_BASE_URL}/${id}`, memberData);
      return response.data;
    } catch (error) {
      console.error(`Error updating member with ID ${id}:`, error);
      throw error;
    }
  },

  deleteMember: async (id: string): Promise<void> => {
    try {
        await apiClient.delete(`${API_BASE_URL}/${id}`);
    } catch (error) {
      console.error(`Error deleting member with ID ${id}:`, error);
      throw error;
    }
  },

  changeMemberPassword: async (data: PasswordUpdate): Promise<string> => {
    try {
        const response = await apiClient.patch(`${API_BASE_URL}/`, data);
        return response.data;
    } catch (error) {
        console.error(`Error changing member password:`, error);
      throw error;
    }
  }
};
