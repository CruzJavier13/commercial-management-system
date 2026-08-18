import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetProductDto } from '../../../core/models/product.interface';
import { InvoiceDetailDto, CreateInvoiceDto } from '../../../core/models/billing.interface';
import { BillingService } from '../../../core/services/billing-service/billing.service';
import { Router } from '@angular/router';
import { ProductService } from '../../../core/services/product-service/product.service';

@Component({
  standalone: true,
  selector: 'app-billing',
  imports: [CommonModule, FormsModule],
  templateUrl: './billing.html'
})
export class Billing implements OnInit {
  //private productService = inject(ProductService);
 //private billingService = inject(BillingService)
  private router = inject(Router);

  // Filtros y estados del Punto de Venta
  productSearchTerm = '';
  paymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA' = 'EFECTIVO';
  isLoadingProducts = false;

  subTotal = 0;
  taxAmount = 0;
  totalAmount = 0;

  cartItems: InvoiceDetailDto[] = [];

  productsCatalogs: any[] = [];

  constructor(private productService: ProductService, private billingService: BillingService, private cdr: ChangeDetectorRef) { }

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
    const existingItem = this.cartItems.find(item => item.productId === product.id);

    if (existingItem) {
      this.updateQuantity(existingItem, 1);
    } else {
      const newItem: InvoiceDetailDto = {
        productId: product.id,
        productName: product.name,
        productCode: product.productCode,
        quantity: 1,
        unitPrice: product.basePrice, 
        discountAmount: 0,
        lineTotal: product.basePrice
      };
      this.cartItems.push(newItem);
      this.calculateInvoiceTotals();
    }
  }

  updateQuantity(item: InvoiceDetailDto, amount: number): void {
    item.quantity += amount;

    if (item.quantity <= 0) {
      this.cartItems = this.cartItems.filter(i => i.productId !== item.productId);
    } else {
      item.lineTotal = item.quantity * item.unitPrice - item.discountAmount;
    }
    this.calculateInvoiceTotals();
  }

  calculateInvoiceTotals(): void {
    this.subTotal = this.cartItems.reduce((acc, item) => acc + item.lineTotal, 0);
    this.taxAmount = this.subTotal * 0.15; // 15% IVA aplicable en Nicaragua
    this.totalAmount = this.subTotal + this.taxAmount;
  }

  clearCart(): void {
    this.cartItems = [];
    this.calculateInvoiceTotals();
  }

  checkoutInvoice(): void {
    if (this.cartItems.length === 0) {
      alert('El carrito de compras está vacío. No hay artículos para facturar.');
      return;
    }

    const finalInvoicePayload: CreateInvoiceDto = {
      customerId: 1,
      employeeId: 2,
      paymentMethod: this.paymentMethod,
      details: this.cartItems
    };

    this.billingService.createInvoice(finalInvoicePayload).subscribe({
      next: (response) => {
  
        if (response.success) {
          alert('¡Venta procesada con éxito! Stock rebajado en Kárdex y factura asentada contablemente.');
          this.clearCart(); 
        } else {
          alert('Error en transacciones de SQL Server: ' + response.error);
        }
      },
      error: (err) => {
        console.error('Fallo de red al facturar:', err);
        alert('Error crítico de comunicación: El módulo de caja no pudo conectar con la API de .NET.');
      }
    });
  }
}
