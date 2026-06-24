import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetCustomerDto } from '../../../core/models/customer.interface'; 

@Component({
  standalone: true,
  selector: 'app-customers',
  imports: [CommonModule, FormsModule], 
  templateUrl: './customer-list.html'
})
export class CustomerList implements OnInit {
  searchTerm = '';

  customersList: GetCustomerDto[] = [
    {
      id: 1,
      customerCode: 'CLI-NAT-01',
      fullName: 'María Auxiliadora López',
      taxIdentification: '001-221094-1002J', 
      phoneNumber: '+505 8888-1234',
      email: 'maria.lopez@gmail.com',
      address: 'Bello Horizonte, de la rotonda 2c al lago, Managua.',
      isActive: true
    },
    {
      id: 2,
      customerCode: 'CLI-JUR-02',
      fullName: 'Comercializadora e Inversiones del Norte S.A.',
      taxIdentification: 'J0310000444555',
      phoneNumber: '+505 2278-9000',
      email: 'corporativo@inversionesnorte.com',
      address: 'Km 9 Carretera a Masaya, Edificio Invercasa, Piso 3.',
      isActive: true
    }
  ];

  constructor() {}

  ngOnInit(): void {}

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición del cliente ID: ' + id);
  }

  onDelete(id: number): void {
    alert('Ejecutando procedimiento sal.usp_Customers_Delete para el ID: ' + id);
  }
}

