export interface GetRoleDto {
  id: number;
  name: 'ADMINISTRADOR' | 'CAJERO' | 'SUPERVISOR' | 'INVENTARIADO';
  description?: string;
  isActive: boolean;
}

export interface GetUserDto {
  id: number;
  username: string;
  email: string;
  employeeId?: number;
  roleId: number;
  roleName: string;
  isActive: boolean;
}

export interface AuthResponseDto {
  token: string;
  expiration: string;
  user: GetUserDto;  
}