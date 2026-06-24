export interface InvoiceDetailDto {
  id?: number;
  productId: number;
  productName: string;      
  productCode: string;
  quantity: number;
  unitPrice: number;
  discountAmount: number;
  lineTotal: number;
}

export interface GetInvoiceDto {
  id: number;
  invoiceNumber: string;    
  customerId: number;
  customerName: string;
  employeeId: number;       
  employeeName: string;
  subTotal: number;
  taxAmount: number;      
  totalAmount: number;
  paymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA';
  createdAt: string;
  details: InvoiceDetailDto[]; 
}

export interface CreateInvoiceDto {
  customerId: number;
  employeeId: number;
  paymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA';
  details: InvoiceDetailDto[];
}