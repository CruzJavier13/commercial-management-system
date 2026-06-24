import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { GetProductDto } from '../../../core/models/product.interface';
import { InvoiceDetailDto, CreateInvoiceDto } from '../../../core/models/billing.interface';

@Component({
  standalone: true,
  selector: 'app-billing',
  imports: [CommonModule, FormsModule], 
  templateUrl: './billing.html'
})
export class Billing implements OnInit {
  productSearchTerm = '';
  paymentMethod: 'EFECTIVO' | 'TARJETA' | 'TRANSFERENCIA' = 'EFECTIVO';

  // Totales matemáticos financieros
  subTotal = 0;
  taxAmount = 0;
  totalAmount = 0;


  cartItems: InvoiceDetailDto[] = [];


  productsCatalogs: any[] = [
    { id: 101, productCode: 'SKU-APPLE-15PM', name: 'iPhone 15 Pro Max 256GB', basePrice: 1199.00, deviceAttributes: {} },
    { id: 102, productCode: 'SKU-MK-ACET500', name: 'Acetaminofén 500mg MK (Caja 100 Tab)', basePrice: 8.50, medicineAttributes: {} },
    { id: 103, productCode: 'SKU-GEN-MUSE', name: 'Mouse Inalámbrico Ergonómico Logistics', basePrice: 25.00 }
  ];

  constructor() {}

  ngOnInit(): void {}

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
    this.taxAmount = this.subTotal * 0.15; 
    this.totalAmount = this.subTotal + this.taxAmount;
  }

  clearCart(): void {
    this.cartItems = [];
    this.calculateInvoiceTotals();
  }


  checkoutInvoice(): void {
    const finalInvoicePayload: CreateInvoiceDto = {
      customerId: 1, 
      employeeId: 2, 
      paymentMethod: this.paymentMethod,
      details: this.cartItems
    };

    alert('Despachando Orden de Caja al endpoint de .NET (sal.Invoices):\n' + JSON.stringify(finalInvoicePayload));
    this.clearCart();
  }
}
