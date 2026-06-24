import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms'; // 
import { GetEmployeeDto } from '../../../core/models/employee.interface'; 

@Component({
  standalone: true,
  selector: 'app-employee-list',
  imports: [CommonModule, FormsModule], 
  templateUrl: './employee-list.html'
})
export class EmployeeList implements OnInit {
  searchTerm = '';


  employeesList: GetEmployeeDto[] = [
    {
      id: 1,
      employeeCode: 'EMP-ADM-01',
      firstName: 'Jennifer',
      lastName: 'Martínez',
      cardId: '001-120594-1002U', 
      position: 'Gerente General',
      baseSalary: 1250.00,
      phoneNumber: '+505 8888-8888',
      hireDate: new Date(2024, 1, 15).toISOString(),
      isActive: true
    },
    {
      id: 2,
      employeeCode: 'EMP-CAJ-02',
      firstName: 'Carlos',
      lastName: 'Espinoza',
      cardId: '401-200891-1001V',
      position: 'Cajero Principal',
      baseSalary: 450.00,
      phoneNumber: '+505 7777-7777',
      hireDate: new Date(2025, 3, 10).toISOString(),
      isActive: true
    }
  ];

  constructor() {}

  ngOnInit(): void {}

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición del contrato laboral ID: ' + id);
  }

  onDelete(id: number): void {
    alert('Ejecutando procedimiento hr.usp_Employees_Delete para el ID: ' + id);
  }
}
