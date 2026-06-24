import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // 📝 REQUERIDO
import { CreateCategoryDto } from '../../../core/models/category.interface';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], // 🚀 Actívalo aquí
  templateUrl: './category-form.html'
})
export class CategoryForm {
  categoryForm: CreateCategoryDto = {
    categoryCode: '',
    name: '',
    description: ''
  };

  constructor(private router: Router) {}

  onSubmit(): void {

    if (!this.categoryForm.categoryCode.trim()) {
      alert('El código único de la categoría es obligatorio.');
      return;
    }

    if (!this.categoryForm.name.trim()) {
      alert('El nombre comercial de la categoría es obligatorio.');
      return;
    }
   
     const categoryToSave: CreateCategoryDto = {
      categoryCode: this.categoryForm.categoryCode.trim().toUpperCase(),
      name: this.categoryForm.name.trim(),
      description: this.categoryForm.description?.trim() || undefined
    };


    alert('Enviando datos listos a tu API de .NET (ADO.NET):\n' + JSON.stringify(categoryToSave));


    this.router.navigate(['/categories/list']);
  }
}