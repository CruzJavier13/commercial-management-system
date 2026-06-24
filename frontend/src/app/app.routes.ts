import { Routes } from '@angular/router';

export const routes: Routes = [
  { 
    path: '', redirectTo: 'home', pathMatch: 'full' 
  },
  {
    path: 'login',
    loadComponent: () => import('./features/auth/login/login').then(m => m.Login)
  },

  {
    path: '',
    loadComponent: () => import('./features/dashboard/dashboard-layout/dashboard-layout').then(m => m.DashboardLayout),
    children: [

      { path: '', redirectTo: 'home', pathMatch: 'full' },

      {
        path: 'home',
        loadComponent: () => import('./features/dashboard/home-dashboard/home-dashboard').then(m => m.HomeDashboard)
      },

      {
        path: 'products/list',
        loadComponent: () => import('./features/products/product-list/product-list').then(m => m.ProductList)
      },
      {
        path: 'products/create',
        loadComponent: () => import('./features/products/product-form/product-form').then(m => m.ProductForm)
      },

      {
        path: 'categories/list',
        loadComponent: () => import('./features/categories/category-list/category-list').then(m => m.CategoryList)
      },
      {
        path: 'categories/create',
        loadComponent: () => import('./features/categories/category-form/category-form').then(m => m.CategoryForm)
      },

      {
        path: 'suppliers/list',
        loadComponent: () => import('./features/suppliers/supplier-list/supplier-list').then(m => m.SupplierList)
      },
      {
        path: 'suppliers/create',
        loadComponent: () => import('./features/suppliers/supplier-form/supplier-form').then(m => m.SupplierForm)
      },

      {
        path: 'customers/list',
        loadComponent: () => import('./features/customers/customer-list/customer-list').then(m => m.CustomerList)
      },
      {
        path: 'customers/create',
        loadComponent: () => import('./features/customers/customer-form/customer-form').then(m => m.CustomerForm)
      },

      {
        path: 'employees/list',
        loadComponent: () => import('./features/employees/employee-list/employee-list').then(m => m.EmployeeList)
      },
      {
        path: 'employees/create',
        loadComponent: () => import('./features/employees/employee-form/employee-form').then(m => m.EmployeeForm)
      },

      {
        path: 'billing/pos',
        loadComponent: () => import('./features/billing/pos/billing').then(m => m.Billing)
      },
      {
        path: 'billing/history',
        loadComponent: () => import('./features/billing/history/history').then(m => m.History)
      },

      {
        path: 'inventory/dashboard',
        loadComponent: () => import('./features/inventory/inventory-dashboard/inventory-dashboard').then(m => m.InventoryDashboard)
      },
      {
        path: 'inventory/transactions', 
        loadComponent: () => import('./features/inventory/movement-stock/movement-stock').then(m => m.MovementStock)
      },
    ]
  },

  { path: '**', redirectTo: 'home' }
];
