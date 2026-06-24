import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { ProductStockDto } from '../../../core/models/product.interface'; 

@Component({
  standalone: true,
  selector: 'app-inventory-dashboard',
  imports: [CommonModule, FormsModule], // 🚀 Activa ngModel e *ngIf
  templateUrl: './inventory-dashboard.html'
})
export class InventoryDashboard implements OnInit {
  searchTerm = '';

  stocksList: ProductStockDto[] = [
    {
      id: 1,
      productId: 101,
      productCode: 'SKU-APPLE-15PM',
      productName: 'iPhone 15 Pro Max 256GB',
      currentStock: 14,
      minimumRequired: 5,
      maximumAllowed: 30,
      lastUpdated: new Date().toISOString()
    },
    {
      id: 2,
      productId: 102,
      productCode: 'SKU-MK-ACET500',
      productName: 'Acetaminofén 500mg MK (Caja 100 Tab)',
      currentStock: 3, 
      minimumRequired: 10,
      maximumAllowed: 100,
      lastUpdated: new Date().toISOString()
    },
    {
      id: 3,
      productId: 103,
      productCode: 'SKU-GEN-MUSE',
      productName: 'Mouse Inalámbrico Ergonómico Logistics',
      currentStock: 50,
      minimumRequired: 15,
      maximumAllowed: 50,
      lastUpdated: new Date().toISOString()
    }
  ];

  constructor() {}

  ngOnInit(): void {}
}
