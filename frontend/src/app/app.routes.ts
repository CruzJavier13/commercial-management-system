import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
  },
  {
    path: '',
    loadComponent: () => import('./features/dashboard/dashboard-layout/dashboard-layout').then(m => m.DashboardLayout),
    children: [
      { path: '', redirectTo: 'products', pathMatch: 'full' },
      {
        path: 'products',
        loadComponent: () => import('./features/products/product-list/product-list').then(m => m.ProductList)
      },
      {
        path: 'categories',
        loadComponent: () => import('./features/categories/category-list/category-list').then(m => m.CategoryList)
      },
      {
        path: 'suppliers',
        loadComponent: () => import('./features/suppliers/supplier-list/supplier-list').then(m => m.SupplierList)
      },
      {
        path: 'inventory',
        loadComponent: () => import('./features/inventory/inventory-dashboard/inventory-dashboard').then(m => m.InventoryDashboard)
      }
    ]
  },

  { path: '**', redirectTo: 'login' }
];
