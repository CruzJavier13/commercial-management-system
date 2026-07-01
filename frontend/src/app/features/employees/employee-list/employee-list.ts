import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // 
import { GetEmployeeDto } from '../../../core/models/employee.interface'; 
import { EmployeeService } from '../../../core/services/employee-service/employee.service';
import { ConfirmModalComponent } from "../../../shared/components/confirm-modal/confirm-modal";

@Component({
  standalone: true,
  selector: 'app-employee-list',
  imports: [CommonModule, FormsModule, ConfirmModalComponent], 
  templateUrl: './employee-list.html'
})
export class EmployeeList implements OnInit {
  searchTerm = '';
  idToDelete = 0;
  isDeleteModalOpen = false; 

  employeesList: GetEmployeeDto[] = [];

  constructor(private employeeService: EmployeeService) {}

  ngOnInit(): void {
    this.loadEmployees(); 
  }

  loadEmployees(): void {
    this.employeeService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.employeesList = response.data;
          console.log('Lista de personal cargada con éxito:', this.employeesList);
        } else {
          console.error('Error de API al listar personal:', response.error);
        }
      },
      error: (err) => {
        console.error('Fallo crítico de comunicación de red con el backend .NET:', err);
      }
    });
  }

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición del contrato laboral ID: ' + id);
  }

  onDelete(id: number): void {
    this.idToDelete = id;
    this.isDeleteModalOpen = true; 
  }

  executeDeleteLogic(id: number): void {
    this.employeeService.delete(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.isDeleteModalOpen = false;
          alert('¡Baja laboral procesada con éxito y cuenta de acceso deshabilitada!');
          this.loadEmployees(); 
        } else {
          alert('No se pudo desactivar el colaborador: ' + response.error);
        }
      },
      error: (err) => console.error('Error de red en la baja de personal:', err)
    });
  }

  filterEmployees(): GetEmployeeDto[] {
    if (!this.searchTerm.trim()) {
      return this.employeesList;
    }
    const txt = this.searchTerm.toLowerCase().trim();
    return this.employeesList.filter(e => 
      e.firstName.toLowerCase().includes(txt) || 
      e.lastName.toLowerCase().includes(txt) || 
      e.employeeCode.toLowerCase().includes(txt) ||
      e.identificationNumber.toLowerCase().includes(txt) ||
      (e.systemUsername && e.systemUsername.toLowerCase().includes(txt))
    );
  }
}

