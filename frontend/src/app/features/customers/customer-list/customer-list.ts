import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetCustomerDto } from '../../../core/models/customer.interface'; 
import { CustomerService } from '../../../core/services/customer-service/customer.service';
import { ConfirmModalComponent } from "../../../shared/components/confirm-modal/confirm-modal";

@Component({
  standalone: true,
  selector: 'app-customers',
  imports: [CommonModule, FormsModule, ConfirmModalComponent], 
  templateUrl: './customer-list.html'
})
export class CustomerList implements OnInit {
  searchTerm = '';
  
  isDeleteModalOpen = false;
  idToDelete = 0;

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
    this.idToDelete = id;
    this.isDeleteModalOpen = true; 
  }

  executeDeleteLogic(id: number): void {
    this.customerService.delete(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.isDeleteModalOpen = false;
          alert('¡Producto dado de baja con éxito en la base de datos!');
          this.loadCustomers(); 
        } else {
          alert('No se pudo desactivar el artículo: ' + response.error);
        }
      }
    });
  }
  
  filterCustomers(): GetCustomerDto[] {
    if (!this.searchTerm.trim()) {
      return this.customersList;
    }
    const txt = this.searchTerm.toLowerCase().trim();
    return this.customersList.filter(c => 
      c.fullName.toLowerCase().includes(txt) ||  
      c.customerCode.toLowerCase().includes(txt) || 
      c.identificationNumber.toLowerCase().includes(txt)
    );
  }
}

