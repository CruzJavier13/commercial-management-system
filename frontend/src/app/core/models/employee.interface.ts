export interface SessionAuthDto {
  roleId: number;
  systemUsername: string;
  passwordHash?: string | null;
}

export interface GetEmployeeDto {
  id: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  identificationNumber: string;          
  position: string;         
  baseSalary: number;
  phoneNumber?: string;
  createdAt: string;         
  isActive: boolean;
  
  roleId: number | null;
  roleName: string | null; 
  roleCode: string | null;
  systemUsername: string | null;
  passwordHash: string | null;
}

export interface CreateEmployeeDto {
  employeeCode: string;
  firstName: string;
  lastName: string;
  indentificationNumber: string;
  position: number;
  baseSalary: number;
  phoneNumber?: string;
  createdAt: string;

  roleId: number; 
  username: string;
  password: string; 
}

export interface GetRole{
  Id: number;
  RoleCode: string;
  Name: string;
  Description: string;
  IsActive: boolean;
}
