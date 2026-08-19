import { Component, computed, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetInvoiceDto } from '../../../core/models/billing.interface';
import { BillingService } from '../../../core/services/billing-service/billing.service';

@Component({
  standalone: true,
  selector: 'app-history',
  imports: [CommonModule, FormsModule], 
  templateUrl: './history.html'
})
export class History implements OnInit {
  readonly searchTerm = signal('');
  readonly invoicesList = signal<GetInvoiceDto[]>([]);
  readonly isLoading = signal(false);
  readonly errorMessage = signal('');

  readonly filteredInvoices = computed(() => {
    const term = this.searchTerm().trim().toLowerCase();
    const invoices = this.invoicesList();

    if (!term) {
      return invoices;
    }

    return invoices.filter(invoice =>
      [invoice.InvoiceNumber, invoice.CustomerName, invoice.EmployeeName, invoice.PaymentMethod]
        .some(value => String(value ?? '').toLowerCase().includes(term))
    );
  });

  constructor(private billingService: BillingService) {}

  ngOnInit(): void {
    this.loadInvoices();
  }

  loadInvoices(): void {
    this.isLoading.set(true);
    this.errorMessage.set('');

    this.billingService.getInvoiceHistory().subscribe({
      next: (response) => {
        const invoices = Array.isArray(response.data)
          ? response.data.map(invoice => this.normalizeInvoice(invoice))
          : [];
        this.invoicesList.set(invoices);
        this.isLoading.set(false);
      },
      error: (error) => {
        console.error('Error al cargar el historial de facturación:', error);
        this.errorMessage.set('No se pudo cargar el historial de ventas.');
        this.isLoading.set(false);
      }
    });
  }

  private normalizeInvoice(invoice: GetInvoiceDto): GetInvoiceDto {
    const rawInvoice = invoice as unknown as Record<string, unknown>;

    return {
      id: Number(rawInvoice['id'] ?? rawInvoice['Id'] ?? 0),
      InvoiceNumber: String(rawInvoice['InvoiceNumber'] ?? rawInvoice['invoiceNumber'] ?? rawInvoice['PnvoiceNumber'] ?? ''),
      CustomerId: Number(rawInvoice['CustomerId'] ?? rawInvoice['customerId'] ?? rawInvoice['PustomerId'] ?? 0),
      CustomerName: String(rawInvoice['CustomerName'] ?? rawInvoice['customerName'] ?? rawInvoice['PustomerName'] ?? ''),
      EmployeeId: Number(rawInvoice['EmployeeId'] ?? rawInvoice['employeeId'] ?? rawInvoice['PmployeeId'] ?? 0),
      EmployeeName: String(rawInvoice['EmployeeName'] ?? rawInvoice['employeeName'] ?? ''),
      SubTotal: Number(rawInvoice['SubTotal'] ?? rawInvoice['subTotal'] ?? rawInvoice['subTotalAmount'] ?? 0),
      TaxAmount: Number(rawInvoice['TaxAmount'] ?? rawInvoice['taxAmount'] ?? 0),
      TotalAmount: Number(rawInvoice['TotalAmount'] ?? rawInvoice['totalAmount'] ?? rawInvoice['totalBilled'] ?? 0),
      PaymentMethod: (rawInvoice['PaymentMethod'] ?? rawInvoice['paymentMethod'] ?? '') as GetInvoiceDto['PaymentMethod'],
      CreatedAt: String(rawInvoice['CreatedAt'] ?? rawInvoice['createdAt'] ?? rawInvoice['invoiceDate'] ?? ''),
      Details: this.normalizeDetails(rawInvoice['Details'] ?? rawInvoice['details'] ?? [])
    };
  }

  private normalizeDetails(details: unknown): GetInvoiceDto['Details'] {
    if (!Array.isArray(details)) {
      return [];
    }

    return details.map(detail => {
      const rawDetail = detail as Record<string, unknown>;

      return {
        id: Number(rawDetail['id'] ?? rawDetail['Id'] ?? 0),
        ProductId: Number(rawDetail['ProductId'] ?? rawDetail['productId'] ?? 0),
        ProductName: String(rawDetail['ProductName'] ?? rawDetail['productName'] ?? ''),
        ProductCode: String(rawDetail['ProductCode'] ?? rawDetail['productCode'] ?? ''),
        Quantity: Number(rawDetail['Quantity'] ?? rawDetail['quantity'] ?? 0),
        UnitPrice: Number(rawDetail['UnitPrice'] ?? rawDetail['unitPrice'] ?? 0),
        DiscountAmount: Number(rawDetail['DiscountAmount'] ?? rawDetail['discountAmount'] ?? 0),
        LineTotal: Number(rawDetail['LineTotal'] ?? rawDetail['lineTotal'] ?? 0),
        PriceBilled: Number(rawDetail['PriceBilled'] ?? rawDetail['priceBilled'] ?? 0),
        TaxRate: Number(rawDetail['TaxRate'] ?? rawDetail['taxRate'] ?? 0)
      };
    });
  }

  trackByInvoiceId(_index: number, invoice: GetInvoiceDto): number {
    return invoice.id;
  }

  viewInvoiceDetails(invoice: GetInvoiceDto): void {
    const details = invoice.Details.length > 0
      ? invoice.Details.map((detail, index) => [
          `${index + 1}. ${detail.ProductName || 'Producto #' + detail.ProductId}`,
          `Código: ${detail.ProductCode || 'N/D'}`,
          `Cantidad: ${detail.Quantity}`,
          `Precio unitario: C$${detail.UnitPrice.toFixed(2)}`,
          `Descuento: C$${detail.DiscountAmount.toFixed(2)}`,
          `Impuesto: ${detail.TaxRate}%`,
          `Total línea: C$${detail.LineTotal.toFixed(2)}`
        ].join(' | ')).join('\n')
      : 'Sin detalles de productos.';

    alert([
      `Factura: ${invoice.InvoiceNumber}`,
      `Fecha: ${invoice.CreatedAt}`,
      `Cliente: ${invoice.CustomerName || 'Cliente #' + invoice.CustomerId}`,
      `Empleado: ${invoice.EmployeeName || 'Empleado #' + invoice.EmployeeId}`,
      `Método de pago: ${invoice.PaymentMethod}`,
      `Subtotal: C$${invoice.SubTotal.toFixed(2)}`,
      `Impuesto: C$${invoice.TaxAmount.toFixed(2)}`,
      `Total: C$${invoice.TotalAmount.toFixed(2)}`,
      '',
      'DETALLE DE PRODUCTOS',
      details
    ].join('\n'));
  }

  printReport(): void {
    
  }

  startDate: string = '';
  endDate: string = '';

  filterReport(): void {
  }
}