import apiClient from "./apiClient";
import { Admin } from "./adminService";

const API_BASE_URL = "/api/membershipPlan";

export interface MembershipPlan {
  planID: string;
  planName: string;
  planDescription: string;
  planPrice: number;
  forDeletion: boolean;
  adminId: string | null;
  admin: Admin
}

export interface MembershipPlanCreate {
  planName: string;
  planDescription: string;
  planPrice: number;
}

export interface MembershipPlanUpdate {
  planName: string;
  planDescription: string;
  planPrice: number;
}

export const membershipPlanService = {
  getMembershipPlans: async (): Promise<MembershipPlan[]> => {
    try {
      const response = await apiClient.get<MembershipPlan[]>(API_BASE_URL);
      return response.data;
    } catch (error) {
      console.error("Error fetching membership plans:", error);
      throw error;
    }
  },
  getMembershipPlanById: async (id: string): Promise<MembershipPlan> => {
    try {
      const response = await apiClient.get(`${API_BASE_URL}/${id}`);
      return response.data;
    } catch (error) {
      console.error(`Error fetching membership with ID ${id}:`, error);
      throw error;
    }
  },
  createMembershipPlan: async (membershipPlanData : MembershipPlanCreate): Promise<string> => {
    try {
      const response = await apiClient.post(`${API_BASE_URL}`, membershipPlanData);
      return response.data;
    } catch (error) {
      console.error('Error creating membership plan:', error);
      throw error;
    }
  },
  updateMembershipPlan: async (id: string, membershipPlanData: MembershipPlanUpdate): Promise<string> => {
    try {
      const response = await apiClient.put(`${API_BASE_URL}/${id}`, membershipPlanData);
      return response.data;
    } catch (error) {
      console.error(`Error updating membership with ID ${id}:`, error);
      throw error;
    }
  },
  deleteMembershipPlan: async (id: string): Promise<void> => {
  try {
      await apiClient.delete(`${API_BASE_URL}/${id}`);
  } catch (error) {
    console.error(`Error deleting membership Plan with ID ${id}:`, error);
    throw error;
  }
  }
}
