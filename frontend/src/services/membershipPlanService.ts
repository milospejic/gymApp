import axios from "axios";
import { Admin } from "./adminService";

const API_BASE_URL = "https://localhost:7151/api/membershipPlan";

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
      const response = await axios.get<MembershipPlan[]>(API_BASE_URL, {
        headers: {
          Accept: "application/json",
        },
      });
      return response.data;
    } catch (error) {
      console.error("Error fetching membership plans:", error);
      throw error;
    }
  },
  getMembershipPlanById: async (id: string, token: string): Promise<MembershipPlan> => {
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
  createAdmin: async (membershipPlanData : MembershipPlanCreate): Promise<string> => {
    try {
      const response = await axios.post(`${API_BASE_URL}`, membershipPlanData, {
        headers: { 'Content-Type': 'application/json' },
      });
      return response.data;
    } catch (error) {
      console.error('Error creating membership plan:', error);
      throw error;
    }
  },
  updateMembershipPlan: async (id: string, membershipPlanData: MembershipPlanUpdate, token: string): Promise<string> => {
    try {
      const response = await axios.put(`${API_BASE_URL}/${id}`, membershipPlanData, {
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
  },
  deleteMembershipPlan: async (id: string, token: string): Promise<void> => {
  try {
      await axios.delete(`${API_BASE_URL}/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });
  } catch (error) {
    console.error(`Error deleting membership Plan with ID ${id}:`, error);
    throw error;
  }
  }
}