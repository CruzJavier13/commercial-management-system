import { Component, inject, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service/auth.service';

@Component({
  standalone: true,
  selector: 'app-home-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './home-dashboard.html'
})
export class HomeDashboard implements OnInit {
  public authService = inject(AuthService);
  constructor() {}
  ngOnInit(): void {}
}
