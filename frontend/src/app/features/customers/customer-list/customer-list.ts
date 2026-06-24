import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetCustomerDto } from '../../../core/models/customer.interface'; 
import { CustomerService } from '../../../core/services/customer-service/customer.service';

@Component({
  standalone: true,
  selector: 'app-customers',
  imports: [CommonModule, FormsModule], 
  templateUrl: './customer-list.html'
})
export class CustomerList implements OnInit {
  searchTerm = '';
  
  customersList: GetCustomerDto[] = [];

  constructor(private customerService: CustomerService) {}

  ngOnInit(): void {
    this.loadCustomers(); 
  }

  loadCustomers(): void {
    this.customerService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.customersList = response.data; 
        } else {
          console.error('Error reportado por la API:', response.error);
        }
      },
      error: (err) => {
        console.error('Fallo de comunicación con el servidor .NET:', err);
      }
    });
  }

  onEdit(id: number): void {

    alert('Redireccionando de forma independiente a la edición del cliente ID: ' + id);
  }

  onDelete(id: number): void {
    if (confirm('¿Está seguro de que desea desactivar lógicamente este cliente del sistema?')) {
      this.customerService.delete(id).subscribe({
        next: (response) => {
          if (response.success) {
            alert('Cliente desactivado con éxito.');
            this.loadCustomers();
          } else {
            alert('No se pudo desactivar el registro: ' + response.error);
          }
        }
      });
    }
  }
  filterCustomers(): GetCustomerDto[] {
    if (!this.searchTerm.trim()) {
      return this.customersList;
    }
    const txt = this.searchTerm.toLowerCase().trim();
    return this.customersList.filter(c => 
      c.fullName.toLowerCase().includes(txt) || 
      c.customerCode.toLowerCase().includes(txt) || 
      c.taxIdentification.toLowerCase().includes(txt)
    );
  }
}

