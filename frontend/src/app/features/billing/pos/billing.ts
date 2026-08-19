import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetProductDto } from '../../../core/models/product.interface';
import { InvoiceDetailDto, CreateInvoiceDto } from '../../../core/models/billing.interface';
import { BillingService } from '../../../core/services/billing-service/billing.service';
import { Router } from '@angular/router';
import { ProductService } from '../../../core/services/product-service/product.service';
import { AuthService } from '../../../core/services/auth-service/auth.service';

@Component({
  standalone: true,
  selector: 'app-billing',
  imports: [CommonModule, FormsModule],
  templateUrl: './billing.html'
})
export class Billing implements OnInit {
  private router = inject(Router);
  private readonly taxRate = 0.15;

  // Filtros y estados del Punto de Venta
  productSearchTerm = '';
  paymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA' = 'EFECTIVO';
  searchCustomer = '';
  
  isLoadingProducts = false;

  subTotal = 0;
  taxAmount = 0;
  totalAmount = 0;

  cartItems: InvoiceDetailDto[] = [];

  productsCatalogs: any[] = [];

  constructor(private productService: ProductService, 
              private billingService: BillingService,   
              private authService: AuthService, 
              private cdr: ChangeDetectorRef) { }

  ngOnInit(): void {
    this.getProductsFromBackend();
  }


  getProductsFromBackend(search: string = ''): void {
    this.isLoadingProducts = true;
    
    this.productService.getAll(search).subscribe({
      next: (response) => {
 
        this.productsCatalogs = response.data;
        this.isLoadingProducts = false;
        this.cdr.detectChanges();
      },
      error: (err) => {
        console.error('Error al conectar con el catálogo de productos .NET:', err);
        this.isLoadingProducts = false;
        this.cdr.detectChanges();
      }
    });
  }

  onSearchChange(): void {

    this.getProductsFromBackend(this.productSearchTerm);
  }

  addToCart(product: any): void {
    const existingItem = this.cartItems.find(item => item.ProductId === product.id);

    if (existingItem) {
      this.updateQuantity(existingItem, 1);
    } else {
      const newItem: InvoiceDetailDto = {
        ProductId: product.id,
        ProductName: product.name,
        ProductCode: product.productCode,
        Quantity: 1,
        UnitPrice: Number(product.basePrice),
        DiscountAmount: 0,
        LineTotal: Number(product.basePrice)
      };
      this.cartItems.push(newItem);
      this.calculateInvoiceTotals();
    }
  }

  updateQuantity(item: InvoiceDetailDto, amount: number): void {
    item.Quantity += amount;

    if (item.Quantity <= 0) {
      this.cartItems = this.cartItems.filter(i => i.ProductId !== item.ProductId);
    } else {
      item.LineTotal = Number(item.Quantity) * Number(item.UnitPrice) - Number(item.DiscountAmount || 0);
    }
    this.calculateInvoiceTotals();
  }

  calculateInvoiceTotals(): void {
    this.subTotal = this.cartItems.reduce((acc, item) => acc + Number(item.LineTotal || 0), 0);
    this.taxAmount = this.subTotal * this.taxRate;
    this.totalAmount = this.subTotal + this.taxAmount;

    this.subTotal = Number(this.subTotal.toFixed(2));
    this.taxAmount = Number(this.taxAmount.toFixed(2));
    this.totalAmount = Number(this.totalAmount.toFixed(2));
  }

  clearCart(): void {
    this.cartItems = [];
    this.calculateInvoiceTotals();
    this.cdr.detectChanges();
  }


  checkoutInvoice(): void {
    if (this.cartItems.length === 0) {
      alert('El carrito de compras está vacío. No hay artículos para facturar.');
      return;
    }
    const currentEmployeeId = Number(this.authService.currentUser()?.id || 0);
    console.log('ID del empleado actual:', currentEmployeeId);
    const translatedPaymentMethod = 
    this.paymentMethod === 'EFECTIVO' ? 'Cash' :
    this.paymentMethod === 'TARJETA' ? 'Card' : 
    this.paymentMethod === 'TRANSFERENCIA' ? 'Transfer' : 'Cash';
    const finalInvoicePayload: CreateInvoiceDto = {
      CustomerId: 1,
      EmployeeId: currentEmployeeId,
      PaymentMethod: translatedPaymentMethod as any,

      SubTotalAmount: Number(this.subTotal.toFixed(2)),
      TaxAmount: Number(this.taxAmount.toFixed(2)),
      TotalBilled: Number(this.totalAmount.toFixed(2)),

      Details: this.cartItems.map(item => ({
        ProductId: item.ProductId,
        ProductName: item.ProductName,
        ProductCode: item.ProductCode,
        Quantity: Number(item.Quantity),
        UnitPrice: Number(item.UnitPrice),
        DiscountAmount: Number(item.DiscountAmount || 0),
        LineTotal: Number(item.LineTotal),

        PriceBilled: Number(item.UnitPrice),
        TaxRate: this.taxRate * 100
      }))
    };

    this.billingService.createInvoice(finalInvoicePayload).subscribe({
      next: (response) => {
        console.log('Respuesta de la API de facturación:', response);
        if (response?.success !== false) {
          this.clearCart();
          alert('¡Venta procesada con éxito! Stock rebajado en Kárdex y factura asentada contablemente.');
        } else {
          alert('Error en transacciones de SQL Server: ' + (response.error || response.message || 'No se pudo procesar la venta.'));
        }
      },
      error: (err) => {
        console.error('Fallo de red al facturar:', err);
        alert('Error crítico de comunicación: El módulo de caja no pudo conectar con la API de .NET.');
      }
    });
  }

  onSearchCustomer(): void {
    console.log('Búsqueda de cliente:', this.searchCustomer);
  }
}
