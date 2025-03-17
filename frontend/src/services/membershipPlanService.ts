import axios from "axios";

const API_BASE_URL = "https://localhost:7151/api/membershipPlan";

export interface MembershipPlan {
  planID: string;
  planName: string;
  planDescription: string;
  planPrice: number;
}

export const getMembershipPlans = async (): Promise<MembershipPlan[]> => {
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
};
