export interface ProductStockDto {
  id: number;
  productId: number;
  productName: string;      
  productCode: string;
  currentStock: number;
  minimumRequired: number;
  maximumAllowed: number;
  lastUpdated: string;
}

export interface InventoryTransactionDto {
  id: number;
  productId: number;
  productCode: string;      
  productName: string;      
  transactionType: 'ENTRADA' | 'SALIDA' | 'AJUSTE';
  quantity: number;
  referenceDocument: string;
  userId: number;
  userName: string;
  transactionDate: string;
}

export interface GetProductDto {
  id: number;
  productCode: string;
  categoryId: number;
  supplierId: number;
  name: string;
  description?: string | null;
  basePrice: number;
  isVirtualService: boolean;
  isActive: boolean;
  createdAt: string; 
  
  medicineAttributes?: MedicineExtensionDto | null; 
  deviceAttributes?: DeviceExtensionDto | null; 
}

export interface CreateProductDto {
  productCode: string;
  name: string;
  description?: string | null;
  categoryId: number;
  supplierId: number;
  basePrice: number;
  isVirtualService: boolean;

  healthRegisterNumber?: string;
  activeIngredient?: string;
  expirationDateRequired?: boolean;
  requiresPrescription?: boolean;

  brand?: string;
  model?: string;
  serialNumberOrIMEI?: string;
  warrantyPeriodMonths?: number;    
}

export interface MedicineExtensionDto {
  healthRegisterNumber: string;         
  activeIngredient: string;            
  expirationDateRequired: boolean;      
  requiresPrescription: boolean;        
}

export interface DeviceExtensionDto {
  brand: string;                        
  model: string;                        
  serialNumberOrIMEI?: string | null;   
  warrantyPeriodMonths: number;        
}