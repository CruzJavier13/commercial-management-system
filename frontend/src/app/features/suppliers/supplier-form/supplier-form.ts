import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateSupplierDto } from '../../../core/models/supplier'; 
import { SupplierService } from '../../../core/services/supplier-service/supplier.service'; 

@Component({
  standalone: true,
  selector: 'app-supplier-form',
  imports: [CommonModule, FormsModule, RouterLink], 
  templateUrl: './supplier-form.html'
})
export class SupplierForm implements OnInit {
  
  supplierForm: CreateSupplierDto = {
    supplierCode: '',
    companyName: '',
    taxIdentification: '',
    email: '',
    phoneNumber: '',
    address: ''
  };


  constructor(
    private supplierService: SupplierService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {

    if (!this.supplierForm.supplierCode || !this.supplierForm.supplierCode.trim()) {
      alert('El código único del proveedor es obligatorio.');
      return;
    }

    if (!this.supplierForm.taxIdentification || !this.supplierForm.taxIdentification.trim()) {
      alert('La identificación fiscal (RUC / Cédula) es obligatoria.');
      return;
    }

    if (!this.supplierForm.companyName || !this.supplierForm.companyName.trim()) {
      alert('La razón social o nombre de la empresa es obligatoria.');
      return;
    }

    const payloadToSave: CreateSupplierDto = {
      supplierCode: this.supplierForm.supplierCode.trim().toUpperCase(),
      taxIdentification: this.supplierForm.taxIdentification.trim().toUpperCase(),
      companyName: this.supplierForm.companyName.trim(),
      // Si vienen vacíos o con espacios en blanco, enviamos undefined para que .NET lo interprete como NULL
      email: this.supplierForm.email?.trim() ? this.supplierForm.email.trim().toLowerCase() : undefined,
      phoneNumber: this.supplierForm.phoneNumber?.trim() ? this.supplierForm.phoneNumber.trim() : undefined,
      address: this.supplierForm.address?.trim() ? this.supplierForm.address.trim() : undefined
    };

    this.supplierService.create(payloadToSave).subscribe({
      next: (response) => {
        if (response.success) {
          alert('¡Ficha del proveedor guardada con éxito en la base de datos!');
          this.router.navigate(['/suppliers/list']);
        } else {
          alert('Error reportado por el servidor: ' + response.error);
        }
      },
      error: (err) => {
        console.error('Error crítico de comunicación por red:', err);
        alert('Hubo un fallo al conectar con la API de .NET. Revisa si el backend está encendido.');
      }
    });
  }
}