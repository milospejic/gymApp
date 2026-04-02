export interface MembershipPlanDto {
    planID: string;
    planName: string;
    planPrice: number;
    description: string;
}

export interface MembershipDto {
    membershipID: string;
    membershipFrom: string;
    membershipTo: string;
    isFeePaid: boolean;
    membershipFee: number;
    planDuration: number;
    membershipPlan?: MembershipPlanDto;
}

export interface Member {
    memberId: string;
    memberName: string;
    memberSurname: string;
    memberEmail: string; 
    memberPhone: string; 
    membershipId: string;
    membership?: MembershipDto;
}

export interface MembershipCreate {
    planDuration: number;
    membershipPlanId: string; 
}

export interface MemberCreate {
    memberName: string;
    memberSurname: string;
    memberEmail: string;
    memberPhone: string;
    memberHashedPassword: string;
    membership: MembershipCreate;
}

export interface MemberUpdate {
    memberName: string;
    memberSurname: string;
    memberEmail: string;
    memberPhone: string;
}
