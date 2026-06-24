import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, CommonModule],
  templateUrl: './sidebar.html'
})
export class SidebarComponent {
  isProductsOpen = false;
  isCategoriesOpen = false;
  isSuppliersOpen = false;
  isInventoryOpen = false;
  isCustomersOpen = false;
  isEmployeesOpen = false;
  isBillingOpen = false;

  toggleMenu(menu: string): void {
    this.isProductsOpen = menu === 'products' ? !this.isProductsOpen : false;
    this.isCategoriesOpen = menu === 'categories' ? !this.isCategoriesOpen : false;
    this.isSuppliersOpen = menu === 'suppliers' ? !this.isSuppliersOpen : false;
    this.isInventoryOpen = menu === 'inventory' ? !this.isInventoryOpen : false;
    this.isCustomersOpen = menu === 'customers' ? !this.isCustomersOpen : false;
    this.isEmployeesOpen = menu === 'employees' ? !this.isEmployeesOpen : false;
    this.isBillingOpen = menu === 'billing' ? !this.isBillingOpen : false;
  }
}
