export interface Admin {
    adminID: string;
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
}

export interface AdminCreate {
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
    adminHashedPassword: string;
}

export interface AdminUpdate {
    adminName: string;
    adminSurname: string;
    adminEmail: string;
    adminPhone: string;
}
