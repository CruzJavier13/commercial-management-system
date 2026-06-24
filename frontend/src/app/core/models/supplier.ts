export interface GetSupplierDto {
  id: number;
  supplierCode: string;
  companyName: string;
  taxIdentification: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
  isActive: boolean;
}

export interface CreateSupplierDto {
  supplierCode: string;
  companyName: string;
  taxIdentification: string;
  email?: string;
  phoneNumber?: string;
  address?: string;
}