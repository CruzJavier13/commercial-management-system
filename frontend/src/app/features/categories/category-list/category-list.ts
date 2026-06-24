import { Component, OnInit } from '@angular/core';
import { GetCategoryDto } from '../../../core/models/category.interface';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-category-list',
  imports: [CommonModule,FormsModule],
  templateUrl: './category-list.html',
  styleUrl: './category-list.css',
})
export class CategoryList implements OnInit{
  searchTerm = '';
  
  categoriesList: GetCategoryDto[] = [
    { 
      id: 1, 
      categoryCode: 'ELEC', 
      name: 'Electrónica e Informática', 
      description: 'Dispositivos tecnológicos, celulares y accesorios de computación.', 
      isActive: true,
      createdAt: new Date().toISOString()
    },
    { 
      id: 2, 
      categoryCode: 'FARM', 
      name: 'Medicamentos y Farmacéuticos', 
      description: 'Productos farmacéuticos, medicamentos de venta libre y suplementos nutricionales.', 
      isActive: true,
      createdAt: new Date().toISOString()
    }
  ];

  constructor() {}

  ngOnInit(): void {}

  onEdit(id: number): void {
    alert('Redireccionando de forma independiente a la edición de la categoría ID: ' + id);
  }

  onDelete(id: number): void {
    alert('Ejecutando procedimiento prd.usp_Categories_Delete para el ID: ' + id);
  }
}
