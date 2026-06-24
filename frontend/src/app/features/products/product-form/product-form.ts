import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { CreateProductDto } from '../../../core/models/product.interface'; 
import { CategoryService } from '../../../core/services/category-service/category.service';
import { SupplierService } from '../../../core/services/supplier-service/supplier.service';
import { ProductService } from '../../../core/services/product-service/product.service';

@Component({
  standalone: true,
  selector: 'app-product-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './product-form.html'
})
export class ProductForm implements OnInit {
  
  formType: 'GENERAL' | 'MEDICINA' | 'ELECTRONICA' = 'GENERAL';

  productForm: CreateProductDto = {
    productCode: '', name: '', description: '', categoryId: 0, supplierId: 0,
    basePrice: 0, isVirtualService: false, healthRegisterNumber: '', activeIngredient: '',
    expirationDateRequired: false, requiresPrescription: false, brand: '', model: '',
    serialNumberOrIMEI: '', warrantyPeriodMonths: 0
  };


  categories: any[] = [];
  suppliers: any[] = [];

  constructor(
    private categoryService: CategoryService,
    private supplierService: SupplierService,
    private productService: ProductService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.loadFormDependencies(); // 🚀 Carga los combos en segundo plano al abrir la pantalla
  }

  loadFormDependencies(): void {
    this.categoryService.getAll().subscribe({
      next: (res) => { if (res.success) this.categories = res.data; },
      error: (err) => console.error('Error al cargar categorías para el formulario:', err)
    });

    this.supplierService.getAll().subscribe({
      next: (res) => { if (res.success) this.suppliers = res.data; },
      error: (err) => console.error('Error al cargar proveedores para el formulario:', err)
    });
  }

  onCategoryChange(event: Event): void {
    const selectedId = +(event.target as HTMLSelectElement).value;
    const matchedCategory = this.categories.find(c => c.id === selectedId);

    // Muta el formulario comparando con los códigos reales de tu base de datos (ELEC, FARM, etc.)
    if (matchedCategory?.categoryCode === 'FARM') {
      this.formType = 'MEDICINA';
    } else if (matchedCategory?.categoryCode === 'ELEC') {
      this.formType = 'ELECTRONICA';
    } else {
      this.formType = 'GENERAL';
    }
  }

  onSubmit(): void {
    if (!this.productForm.productCode.trim() || !this.productForm.name.trim() || this.productForm.categoryId <= 0 || this.productForm.supplierId <= 0 || this.productForm.basePrice <= 0) {
      alert('Por favor, completa de forma correcta todos los campos obligatorios.');
      return;
    }

    const finalPayload: CreateProductDto = {
      ...this.productForm,
      productCode: this.productForm.productCode.trim().toUpperCase(),
      name: this.productForm.name.trim(),
      description: this.productForm.description?.trim() || undefined,
      healthRegisterNumber: this.formType === 'MEDICINA' ? this.productForm.healthRegisterNumber?.trim().toUpperCase() : undefined,
      activeIngredient: this.formType === 'MEDICINA' ? this.productForm.activeIngredient?.trim() : undefined,
      brand: this.formType === 'ELECTRONICA' ? this.productForm.brand?.trim() : undefined,
      model: this.formType === 'ELECTRONICA' ? this.productForm.model?.trim() : undefined,
      serialNumberOrIMEI: this.formType === 'ELECTRONICA' ? this.productForm.serialNumberOrIMEI?.trim().toUpperCase() : undefined
    };

    this.productService.create(finalPayload).subscribe({
      next: (res) => {
        if (res.success) {
          alert('¡Ficha técnica de producto guardada transaccionalmente con éxito!');
          this.router.navigate(['/products/list']);
        } else {
          alert('Error en base de datos: ' + res.error);
        }
      },
      error: (err) => alert('Error crítico de comunicación con el servidor .NET.')
    });
  }
}