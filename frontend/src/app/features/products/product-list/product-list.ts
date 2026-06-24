import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GetProductDto } from '../../../core/models/product.interface';

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, FormsModule],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList {
  searchTerm = '';

  productsList: GetProductDto[] = [
    {
      id: 1,
      productCode: 'SKU-APPLE-15PM',
      categoryId: 3,
      supplierId: 1,
      name: 'iPhone 15 Pro Max 256GB',
      description: 'Dispositivo tecnológico color titanio natural con empaque de fábrica.',
      basePrice: 1199.99,
      isVirtualService: false,
      isActive: true,
      createdAt: new Date().toISOString(),
      deviceAttributes: {
        brand: 'Apple',
        model: 'iPhone 15 Pro Max',
        serialNumberOrIMEI: 'IMEI-019283746554321',
        warrantyPeriodMonths: 12
      }
    },
    {
      id: 2,
      productCode: 'SKU-MK-ACET500',
      categoryId: 2,
      supplierId: 2,
      name: 'Acetaminofén 500mg MK',
      description: 'Analgésico de venta libre. Caja con blisters termoformados.',
      basePrice: 7.25,
      isVirtualService: false,
      isActive: true,
      createdAt: new Date().toISOString(),

      medicineAttributes: {
        healthRegisterNumber: 'RNS-MINSA-2026-0092',
        activeIngredient: 'Acetaminofén 500mg',
        expirationDateRequired: true,
        requiresPrescription: false
      }
    }
  ];

  constructor() {}

  ngOnInit(): void {}

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición del producto ID: ' + id);
  }

  onDelete(id: number): void {
    alert('Ejecutando procedimiento prd.usp_Products_Delete para el ID: ' + id);
  }
}
