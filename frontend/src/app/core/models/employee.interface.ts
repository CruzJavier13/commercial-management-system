export interface GetEmployeeDto {
  id: number;
  employeeCode: string;
  firstName: string;
  lastName: string;
  cardId: string;           // Número de cédula física
  position: string;         // Cargo en la tienda (Ej: Cajero)
  baseSalary: number;
  phoneNumber?: string;
  hireDate: string;         // Fecha de ingreso ISO
  isActive: boolean;
}

export interface CreateEmployeeDto {
  employeeCode: string;
  firstName: string;
  lastName: string;
  cardId: string;
  position: string;
  baseSalary: number;
  phoneNumber?: string;
  hireDate: string;

  roleName: 'ADMINISTRADOR' | 'CAJERO' | 'SUPERVISOR' | 'INVENTARIADO' | ''; 
  username: string;
  password: string; 
}