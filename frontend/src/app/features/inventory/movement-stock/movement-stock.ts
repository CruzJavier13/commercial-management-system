import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { InventoryTransactionDto } from '../../../core/models/product.interface'; 

@Component({
  standalone: true,
  selector: 'app-movement-stock',
  imports: [CommonModule, FormsModule], 
  templateUrl: './movement-stock.html'
})
export class MovementStock implements OnInit {
  searchTerm = '';
  
  isPanelOpen = false;

  movementForm = {
    productId: 0,
    transactionType: 'ENTRADA' as 'ENTRADA' | 'SALIDA' | 'AJUSTE',
    quantity: 1,
    referenceDocument: ''
  };

  productsListDropdown = [
    { id: 101, name: 'iPhone 15 Pro Max 256GB', code: 'SKU-APPLE-15PM' },
    { id: 102, name: 'Acetaminofén 500mg MK (Caja 100 Tab)', code: 'SKU-MK-ACET500' }
  ];

  transactionsList: InventoryTransactionDto[] = [
    {
      id: 1, productId: 101, productCode: 'SKU-APPLE-15PM', productName: 'iPhone 15 Pro Max 256GB',
      transactionType: 'ENTRADA', quantity: 10, referenceDocument: 'COM-2026-0089',
      userId: 1, userName: 'Jennifer Martínez', transactionDate: new Date(2026, 5, 18, 9, 30).toISOString()
    },
    {
      id: 2, productId: 102, productCode: 'SKU-MK-ACET500', productName: 'Acetaminofén 500mg MK (Caja 100 Tab)',
      transactionType: 'SALIDA', quantity: 2, referenceDocument: 'FAC-0000189',
      userId: 2, userName: 'Carlos Espinoza (Cajero)', transactionDate: new Date(2026, 5, 20, 14, 15).toISOString()
    }
  ];

  constructor() {}

  ngOnInit(): void {}


  openCreateMovement() {
    this.isPanelOpen = true;
    this.resetForm();
  }

  closePanel() {
    this.isPanelOpen = false;
  }

  resetForm() {
    this.movementForm = { productId: 0, transactionType: 'ENTRADA', quantity: 1, referenceDocument: '' };
  }

  onSubmitMovement() {

    if (this.movementForm.productId <= 0) return alert('Debes seleccionar un artículo del catálogo.');
    if (this.movementForm.quantity <= 0) return alert('La cantidad debe ser mayor a cero.');
    if (!this.movementForm.referenceDocument.trim()) return alert('El documento de referencia (Factura, Acta, Ajuste) es obligatorio.');


    alert('Despachando movimiento manual de Kárdex a la base de datos de .NET:\n' + JSON.stringify(this.movementForm));
    
    this.closePanel();
  }
}