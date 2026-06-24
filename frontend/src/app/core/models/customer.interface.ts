export interface GetCustomerDto {
  id: number;
  customerCode: string;
  fullName: string;
  taxIdentification: string; // Cédula o RUC Nicaragüense
  phoneNumber?: string;
  email?: string;
  address?: string;
  isActive: boolean;
}

export interface CreateCustomerDto {
  customerCode: string;
  fullName: string;
  taxIdentification: string;
  phoneNumber?: string;
  email?: string;
  address?: string;
}