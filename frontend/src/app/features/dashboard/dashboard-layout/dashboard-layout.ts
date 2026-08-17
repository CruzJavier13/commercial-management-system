import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { SidebarComponent } from "../../../shared/components/sidebar/sidebar";
import { AuthService } from '../../../core/services/auth-service/auth.service';

@Component({
  selector: 'app-dashboard-layout',
  imports: [RouterOutlet, SidebarComponent],
  templateUrl: './dashboard-layout.html',
  styleUrl: './dashboard-layout.css',
})
export class DashboardLayout {
  public authService = inject(AuthService);
}
