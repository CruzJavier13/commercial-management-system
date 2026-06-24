export interface GetCategoryDto {
  id: number;
  categoryCode: string;
  name: string;
  description?: string;
  isActive: boolean;
  createdAt: string;
}

export interface CreateCategoryDto {
  categoryCode: string;
  name: string;
  description?: string;
}