export interface InvoiceDetailDto {
  id?: number;
  ProductId: number;
  ProductName: string;      
  ProductCode: string;
  Quantity: number;
  UnitPrice: number;
  DiscountAmount: number;
  LineTotal: number;

  PriceBilled?: number; 
  TaxRate?: number;  
}

export interface GetInvoiceDto {
  id: number;
  InvoiceNumber: string;    
  CustomerId: number;
  CustomerName: string;
  EmployeeId: number;       
  EmployeeName: string;
  SubTotal: number;
  TaxAmount: number;      
  TotalAmount: number;
  PaymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA' | 'Cash' | 'Card' | 'Transfer';
  CreatedAt: string;
  Details: InvoiceDetailDto[]; 
}

export interface CreateInvoiceDto {
  CustomerId: number;
  EmployeeId: number;
  PaymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA';

  SubTotalAmount: number;
  TaxAmount: number;
  TotalBilled: number;
  
  Details: InvoiceDetailDto[];
}