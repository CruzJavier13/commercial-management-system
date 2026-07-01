import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router'; // 
import { CreateCategoryDto } from '../../../core/models/category.interface';
import { CategoryService } from '../../../core/services/category-service/category.service';

@Component({
  selector: 'app-category-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink], 
  templateUrl: './category-form.html'
})
export class CategoryForm {
  categoryForm: CreateCategoryDto = {
    categoryCode: '',
    name: '',
    description: ''
  };

  constructor(
    private categoryService: CategoryService,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit(): void {

    if (!this.categoryForm.categoryCode || !this.categoryForm.categoryCode.trim()) {
      alert('El código único de la categoría es obligatorio (Ej: FARM).');
      return;
    }

    if (!this.categoryForm.name || !this.categoryForm.name.trim()) {
      alert('El nombre descriptivo de la categoría es obligatorio.');
      return;
    }

    const payloadToSave: CreateCategoryDto = {
      categoryCode: this.categoryForm.categoryCode.trim().toUpperCase(),
      name: this.categoryForm.name.trim(),
  
      description: this.categoryForm.description?.trim() ? this.categoryForm.description.trim() : undefined
    };

    this.categoryService.create(payloadToSave).subscribe({
      next: (response) => {
        if (response.success) {
          alert('¡Categoría comercial registrada con éxito en Variedades Mendoza!');
          this.router.navigate(['/categories/list']);
        } else {
          alert('Error reportado por SQL Server: ' + response.error);
        }
      },
      error: (err) => {
        console.error('Fallo crítico de comunicación con el backend .NET:', err);
        alert('Hubo un error de red al intentar guardar. Revisa si el servidor de la API está encendido.');
      }
    });
  }

  
}