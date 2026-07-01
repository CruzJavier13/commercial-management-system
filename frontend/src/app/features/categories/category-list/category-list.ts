import { Component, OnInit } from '@angular/core';
import { CreateCategoryDto, GetCategoryDto } from '../../../core/models/category.interface';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../../core/services/category-service/category.service';
import { ConfirmModalComponent } from "../../../shared/components/confirm-modal/confirm-modal";

@Component({
  selector: 'app-category-list',
  imports: [CommonModule, FormsModule, ConfirmModalComponent],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css',
})
export class CategoryList implements OnInit{
  searchTerm = '';
  Category: GetCategoryDto[] = [];

  isDeleteModalOpen = false;
  idToDelete = 0;

  isModalOpen = false;
  selectedCategoryId: number = 0;
  
  editForm: CreateCategoryDto = {
    categoryCode: '',
    name: '',
    description: ''
  };

  constructor(private categoryService: CategoryService) {}

  ngOnInit(): void {
    this.loadCategories(); 
  }

  loadCategories(): void {
    this.categoryService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.Category = response.data; 
        }
      },
      error: (err) => console.error('Fallo de red en la grilla:', err)
    });
  }

  openEditModal(categorySelected: GetCategoryDto): void {
    this.selectedCategoryId = categorySelected.id;
    
    // Inyección de memoria en caliente
    this.editForm = {
      categoryCode: categorySelected.categoryCode,
      name: categorySelected.name,
      description: categorySelected.description || ''
    };
    
    this.isModalOpen = true;
  }

  closeModal(): void {
    this.isModalOpen = false;
    this.selectedCategoryId = 0;
  }

  onUpdateSubmit(): void {
    if (!this.editForm.name?.trim()) return alert('El nombre de la categoría es obligatorio.');

    const payloadToUpdate: CreateCategoryDto = {
      categoryCode: this.editForm.categoryCode,
      name: this.editForm.name.trim(),
      description: this.editForm.description?.trim() ? this.editForm.description.trim() : undefined
    };

    this.categoryService.update(this.selectedCategoryId, payloadToUpdate).subscribe({
      next: (response) => {
        if (response.success) {
          alert('¡Ficha de categoría modificada exitosamente en SQL Server!');
          

          this.closeModal();
          
          this.loadCategories(); 
        } else {
          alert('Error reportado por la API: ' + response.error);
        }
      },
      error: (err) => {
        console.error('Fallo crítico de red:', err);
        alert('Fallo de comunicación con el backend .NET.');
      }
    })};

  onDelete(id: number): void {
    this.idToDelete = id;
    this.isDeleteModalOpen = true;
  }

   executeDeleteLogic(id: number): void {
    this.categoryService.delete(id).subscribe({
      next: (res) => {
        if (res.success) {
          this.isDeleteModalOpen = false; 
          this.loadCategories(); 
        }
      }
    });
  }

  filterCategories(): GetCategoryDto[] {
    if (!this.searchTerm.trim()) return this.Category;
    const txt = this.searchTerm.toLowerCase().trim();
    return this.Category.filter(c => 
      c.name.toLowerCase().includes(txt) || c.categoryCode.toLowerCase().includes(txt)
    );
  }
}
