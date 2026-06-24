import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetInvoiceDto } from '../../../core/models/billing.interface'; 

@Component({
  standalone: true,
  selector: 'app-history',
  imports: [CommonModule, FormsModule], 
  templateUrl: './history.html'
})
export class History implements OnInit {
  searchTerm = '';


  invoicesList: GetInvoiceDto[] = [
    {
      id: 1,
      invoiceNumber: 'FAC-0000188',
      customerId: 1,
      customerName: 'María Auxiliadora López',
      employeeId: 2,
      employeeName: 'Carlos Espinoza (Caja 1)',
      subTotal: 25.00,
      taxAmount: 3.75,
      totalAmount: 28.75,
      paymentMethod: 'EFECTIVO',
      createdAt: new Date(2026, 5, 20, 10, 14).toISOString(),
      details: []
    },
    {
      id: 2,
      invoiceNumber: 'FAC-0000189',
      customerId: 2,
      customerName: 'Comercializadora del Norte S.A.',
      employeeId: 2,
      employeeName: 'Carlos Espinoza (Caja 1)',
      subTotal: 1199.00,
      taxAmount: 179.85,
      totalAmount: 1378.85,
      paymentMethod: 'TRANSFERENCIA',
      createdAt: new Date(2026, 5, 20, 14, 20).toISOString(),
      details: []
    }
  ];

  constructor() {}

  ngOnInit(): void {}

  viewInvoiceDetails(invoice: GetInvoiceDto): void {

    alert('Desplegando desglose atómico de artículos para la factura: ' + invoice.invoiceNumber);
  }
}