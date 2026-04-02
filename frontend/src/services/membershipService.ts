import apiClient from './apiClient';
import { Membership, MembershipUpdate } from '../interfaces';

const API_BASE_URL = '/api/membership'; 

export const membershipService = {
    getAllMemberships: async (): Promise<Membership[]> => {
        try {
          const response = await apiClient.get(`${API_BASE_URL}`);
          return response.data;
        } catch (error) {
          console.error('Error fetching memberships:', error);
          throw error;
        }
      },
    getMembershipById: async (id: string): Promise<Membership> => {
        try {
          const response = await apiClient.get(`${API_BASE_URL}/${id}`);
          return response.data;
        } catch (error) {
          console.error(`Error fetching membership with ID ${id}:`, error);
          throw error;
        }
    },
    updateMembership: async (id: string, membershipData: MembershipUpdate): Promise<string> => {
        try {
          const response = await apiClient.put(`${API_BASE_URL}/${id}`, membershipData);
          return response.data;
        } catch (error) {
          console.error(`Error updating membership with ID ${id}:`, error);
          throw error;
        }
    },
    cancelMembership: async (id: string): Promise<string> => {
        try {
          const response = await apiClient.post(`${API_BASE_URL}/${id}/cancel`);
          return response.data;
        } catch (error) {
          console.error(`Error cancelling membership with ID ${id}:`, error);
          throw error;
        }
    }
}