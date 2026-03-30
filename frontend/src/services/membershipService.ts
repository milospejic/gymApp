import apiClient from './apiClient';

const API_BASE_URL = '/api/membership'; 

export interface Membership {
    membershipId: string;
    membershipFrom: string;
    membershipTo: string;
    planDuration: number; 
    membershipFee: string; 
    isFeePaid: string;  
    membershipPlanId: string;
}

export interface MembershipUpdate {
    planDuration: number;
    membershipPlanId: string; 
}

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
    }
}
