export interface Membership {
    membershipId: string;
    membershipFrom: string;
    membershipTo: string;
    planDuration: number; 
    membershipFee: number; 
    isFeePaid: boolean;    
    membershipPlanId: string;
}

export interface MembershipUpdate {
    planDuration: number;
    membershipPlanId: string; 
}
