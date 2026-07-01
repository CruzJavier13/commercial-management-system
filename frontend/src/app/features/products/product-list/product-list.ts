import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { GetProductDto } from '../../../core/models/product.interface';
import { ProductService } from '../../../core/services/product-service/product.service';
import { ConfirmModalComponent } from "../../../shared/components/confirm-modal/confirm-modal";

@Component({
  selector: 'app-product-list',
  imports: [CommonModule, FormsModule, ConfirmModalComponent],
  templateUrl: './product-list.html',
  styleUrl: './product-list.css',
})
export class ProductList {
  searchTerm = '';

  isDeleteModalOpen = false;
  idToDelete = 0;

  productsList!: GetProductDto[];
  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.loadProducts();
  }

  loadProducts(): void {
    this.productService.getAll().subscribe({
      next: (response) => {
        if (response.success) {
          this.productsList = response.data;
        } else {
          console.error('Error reportado por el servidor API:', response.error);
        }
      },
      error: (err) => {
        console.error('Fallo crítico de comunicación de red con el backend:', err);
      }
    });
  }

  onEdit(id: number): void {
    alert('Navegando de forma independiente a la edición de la ficha técnica ID: ' + id);
  }

  onDelete(id: number): void {
    this.idToDelete = id;
    this.isDeleteModalOpen = true; 
  }


  executeDeleteLogic(id: number): void {
    this.productService.delete(id).subscribe({
      next: (response) => {
        if (response.success) {
          this.isDeleteModalOpen = false; 
          alert('¡Producto dado de baja con éxito en la base de datos!');
          this.loadProducts();
        } else {
          alert('No se pudo desactivar el artículo: ' + response.error);
        }
      }
    });
  }

  filterProducts(): GetProductDto[] {
    if (!this.searchTerm.trim()) {
      return this.productsList;
    }
    const txt = this.searchTerm.toLowerCase().trim();
    return this.productsList.filter(p => 
      p.name.toLowerCase().includes(txt) || 
      p.productCode.toLowerCase().includes(txt) ||
      p.medicineAttributes?.activeIngredient.toLowerCase().includes(txt) ||
      p.deviceAttributes?.brand.toLowerCase().includes(txt) ||
      p.deviceAttributes?.model.toLowerCase().includes(txt)
    );
  }
}
