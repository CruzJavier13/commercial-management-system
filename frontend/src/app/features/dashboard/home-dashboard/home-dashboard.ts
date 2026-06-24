import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-home-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './home-dashboard.html'
})
export class HomeDashboard implements OnInit {
  constructor() {}
  ngOnInit(): void {}
}
