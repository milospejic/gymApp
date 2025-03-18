import axios from 'axios';

const API_BASE_URL = 'https://localhost:7151/api/membership'; // Update with your backend URL

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
    getAllMemberships: async (token: string): Promise<Membership[]> => {
        try {
          const response = await axios.get(`${API_BASE_URL}`, {
            headers: { Authorization: `Bearer ${token}` },
          });
          return response.data;
        } catch (error) {
          console.error('Error fetching memberships:', error);
          throw error;
        }
      },
    getMembershipById: async (id: string, token: string): Promise<Membership> => {
        try {
          const response = await axios.get(`${API_BASE_URL}/${id}`, {
            headers: { Authorization: `Bearer ${token}` },
          });
          return response.data;
        } catch (error) {
          console.error(`Error fetching membership with ID ${id}:`, error);
          throw error;
        }
    },
    updateMembership: async (id: string, membershipData: MembershipUpdate, token: string): Promise<string> => {
        try {
          const response = await axios.put(`${API_BASE_URL}/${id}`, membershipData, {
            headers: { 
              'Content-Type': 'application/json',
              Authorization: `Bearer ${token}`,
            },
          });
          return response.data;
        } catch (error) {
          console.error(`Error updating membership with ID ${id}:`, error);
          throw error;
        }
    }
}