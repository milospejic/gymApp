import apiClient from "./apiClient";
import { MembershipPlan, MembershipPlanCreate, MembershipPlanUpdate } from "../interfaces";

const API_BASE_URL = "/api/membershipPlan";

export const membershipPlanService = {
  getAllPlans: async (): Promise<MembershipPlan[]> => {
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
      console.error(`Error fetching membership plan with ID ${id}:`, error);
      throw error;
    }
  },
  
  createMembershipPlan: async (membershipPlanData: MembershipPlanCreate): Promise<string> => {
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
      console.error(`Error updating membership plan with ID ${id}:`, error);
      throw error;
    }
  },
  

  togglePlanDeletion: async (id: string): Promise<void> => {
    try {
      await apiClient.patch(`${API_BASE_URL}/delete/${id}`);
    } catch (error) {
      console.error(`Error toggling deletion status for plan ID ${id}:`, error);
      throw error;
    }
  }
};