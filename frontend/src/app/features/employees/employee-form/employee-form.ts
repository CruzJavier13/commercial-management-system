import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateEmployeeDto } from '../../../core/models/employee.interface'; 

@Component({
  standalone: true,
  selector: 'app-employee-form',
  imports: [CommonModule, FormsModule, RouterLink], 
  templateUrl: './employee-form.html'
})
export class EmployeeForm implements OnInit {

  employeeForm: CreateEmployeeDto = {
    employeeCode: '',
    firstName: '',
    lastName: '',
    cardId: '',
    position: '',
    baseSalary: 0,
    phoneNumber: '',
    hireDate: new Date().toISOString().substring(0, 10),
    
    roleName: '',
    username: '',
    password: ''
  };

  constructor(private router: Router) {}

  ngOnInit(): void {}

  onSubmit(): void {

    if (!this.employeeForm.employeeCode?.trim()) return alert('El código de empleado es obligatorio.');
    if (!this.employeeForm.cardId?.trim()) return alert('La cédula de identidad del colaborador es obligatoria.');
    if (!this.employeeForm.firstName?.trim() || !this.employeeForm.lastName?.trim()) return alert('Los nombres y apellidos completos son obligatorios.');
    if (this.employeeForm.baseSalary <= 0) return alert('El salario mensual debe ser un monto mayor a cero.');
    
    if (!this.employeeForm.roleName) return alert('Debes asignar un Rol de Acceso al sistema.');
    if (!this.employeeForm.username?.trim()) return alert('El Nombre de Usuario para el login es obligatorio.');
    if (!this.employeeForm.password || this.employeeForm.password.length < 6) return alert('La contraseña temporal es obligatoria y debe tener mínimo 6 caracteres.');

    const finalPayload = {
      ...this.employeeForm,
      employeeCode: this.employeeForm.employeeCode.trim().toUpperCase(),
      cardId: this.employeeForm.cardId.trim().toUpperCase(),
      firstName: this.employeeForm.firstName.trim(),
      lastName: this.employeeForm.lastName.trim(),
      username: this.employeeForm.username.trim().toLowerCase(),
      hireDate: new Date(this.employeeForm.hireDate).toISOString()
    };

    alert('Despachando payload consolidado (Contrato + Cuenta Seguridad) a .NET:\n' + JSON.stringify(finalPayload));
    
    this.router.navigate(['/employees/list']);
  }
}