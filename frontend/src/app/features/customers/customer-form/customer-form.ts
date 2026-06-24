import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateCustomerDto } from '../../../core/models/customer.interface'; 

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
    taxIdentification: '',
    email: '',
    phoneNumber: '',
    address: ''
  };

  constructor(private router: Router) {}

  ngOnInit(): void {}

  onSubmit(): void {

    if (!this.customerForm.customerCode || !this.customerForm.customerCode.trim()) {
      alert('El código único del cliente es obligatorio.');
      return;
    }

    if (!this.customerForm.taxIdentification || !this.customerForm.taxIdentification.trim()) {
      alert('La identificación fiscal (Cédula / RUC) es obligatoria.');
      return;
    }

    if (!this.customerForm.fullName || !this.customerForm.fullName.trim()) {
      alert('El nombre completo o razón social del cliente es obligatorio.');
      return;
    }

    const payloadToSave: CreateCustomerDto = {
      customerCode: this.customerForm.customerCode.trim().toUpperCase(),
      taxIdentification: this.customerForm.taxIdentification.trim().toUpperCase(),
      fullName: this.customerForm.fullName.trim(),
      email: this.customerForm.email?.trim() ? this.customerForm.email.trim().toLowerCase() : undefined,
      phoneNumber: this.customerForm.phoneNumber?.trim() ? this.customerForm.phoneNumber.trim() : undefined,
      address: this.customerForm.address?.trim() ? this.customerForm.address.trim() : undefined
    };

    alert('Despachando payload de cliente hacia .NET mediante ADO.NET:\n' + JSON.stringify(payloadToSave));
    
    this.router.navigate(['/customers/list']);
  }
}