import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; 
import { GetSupplierDto } from '../../../core/models/supplier'; 
import { SupplierService } from '../../../core/services/supplier-service/supplier.service'; 

@Component({
  standalone: true,
  selector: 'app-supplier-list',
  imports: [CommonModule, FormsModule], 
  templateUrl: './supplier-list.html'
})
export class SupplierList implements OnInit {
  searchTerm = '';
  
  suppliersList: GetSupplierDto[] = [];
  constructor(private supplierService: SupplierService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.supplierService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.suppliersList = response.data; // Desglosa el JSON devuelto por tu controlador .NET
          this.cdr.detectChanges();
        } else {
          console.error('Error reportado por la API:', response.error);
        }
      },
      error: (err) => {
        console.error('Fallo crítico de comunicación de red con el servidor .NET:', err);
      }
    });
  }

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición del distribuidor ID: ' + id);
  }

  onDelete(id: number): void {
    if (confirm('¿Está seguro de que desea desactivar lógicamente este proveedor comercial?')) {
      this.supplierService.delete(id).subscribe({
        next: (response) => {
          if (response.success) {
            alert('Proveedor desactivado con éxito (Soft Delete).');
            this.loadSuppliers(); 
          }
        }
      });
    }
  }
}