import { Component, OnInit } from '@angular/core';
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
  
  //suppliersList: GetSupplierDto[] = [];
  suppliersList: GetSupplierDto[] = [
    {
      id: 1,
      supplierCode: 'SUP-DIST-01',
      taxIdentification: 'J031000012345',
      companyName: 'Distribuidora Comercial de Nicaragua S.A. (DISNIC)',
      email: 'ventas@disnic.com.ni',
      phoneNumber: '+505 2222-1111',
      address: 'Km 6.5 Carretera Norte, Managua, Nicaragua.',
      isActive: true
    },
    {
      id: 2,
      supplierCode: 'SUP-FARM-02',
      taxIdentification: 'J031000098765',
      companyName: 'Corporación Farmacéutica Latinoamericana (FarmaCorp)',
      email: 'pedidos@farmacorp.com',
      phoneNumber: '+505 2277-4444',
      address: 'Pista Jean Paul Genie, Edificio Escala, Managua.',
      isActive: true
    },
    {
      id: 3,
      supplierCode: 'SUP-TECH-03',
      taxIdentification: 'J031000055555',
      companyName: 'Tecnologías Globales de Centroamérica S.A.',
      email: 'soporte@techglobal.com',
      phoneNumber: '+505 2255-8888',
      address: 'Altamira, de los semáforos 2 cuadras al sur, Managua.',
      isActive: true
    }
  ];
  constructor(private supplierService: SupplierService) {}

  ngOnInit(): void {
    this.loadSuppliers();
  }

  loadSuppliers(): void {
    this.supplierService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.suppliersList = response.data; // Desglosa el JSON devuelto por tu controlador .NET
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