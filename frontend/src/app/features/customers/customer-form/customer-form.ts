import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateCustomerDto } from '../../../core/models/customer.interface'; 
import { CustomerService } from '../../../core/services/customer-service/customer.service';

@Component({
  standalone: true,
  selector: 'app-customer-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './customer-form.html'
})
export class CustomerForm implements OnInit {
  
  customerForm: CreateCustomerDto = {
    customerCode: '',
    fullName: '',
    identificationNumber: '',
    email: '',
    phoneNumber: '',
    address: ''
  };


  constructor(
    private customerService: CustomerService, 
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {

    if (!this.customerForm.customerCode || !this.customerForm.customerCode.trim()) {
      alert('El código único del cliente es obligatorio.');
      return;
    }

    if (!this.customerForm.identificationNumber || !this.customerForm.identificationNumber.trim()) {
      alert('La identificación fiscal (Cédula / RUC) es obligatoria.');
      return;
    }

    if (!this.customerForm.fullName || !this.customerForm.fullName.trim()) {
      alert('El nombre completo o razón social del cliente es obligatorio.');
      return;
    }

    const payloadToSave: CreateCustomerDto = {
      customerCode: this.customerForm.customerCode.trim().toUpperCase(),
      identificationNumber: this.customerForm.identificationNumber.trim().toUpperCase(),
      fullName: this.customerForm.fullName?.trim() || '',
      email: this.customerForm.email?.trim() ? this.customerForm.email.trim().toLowerCase() : '',
      phoneNumber: this.customerForm.phoneNumber?.trim() ? this.customerForm.phoneNumber.trim() : '',
      address: this.customerForm.address?.trim() ? this.customerForm.address.trim() : ''
    };


    this.customerService.create(payloadToSave).subscribe({
      next: (response) => {
        if (response.success) {
          alert('¡Ficha del cliente registrada con éxito en la base de datos de Variedades Mendoza!');
          this.router.navigate(['/customers/list']);
        } else {
          alert('Error reportado por SQL Server: ' + response.error);
        }
      },
      error: (err) => {
        console.error('Error crítico de red al conectar al backend .NET:', err);
        alert('Hubo un fallo crítico al conectar con la API. Revisa si el backend de .NET está encendido.');
      }
    });
  }
}