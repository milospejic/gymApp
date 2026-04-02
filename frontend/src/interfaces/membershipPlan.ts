import { Admin } from './admin';

export interface MembershipPlan {
  planID: string;
  planName: string;
  planDescription: string;
  planPrice: number;
  forDeletion: boolean;
  adminID: string | null; 
  admin?: Admin;         
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
