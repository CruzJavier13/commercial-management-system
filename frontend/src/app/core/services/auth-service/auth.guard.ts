import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from './auth.service';

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);
  const router = inject(Router);

  const token = localStorage.getItem('access_token');

  if (!token) {

    router.navigate(['/login']);
    return false;
  }

  const user = authService.currentUser();

  if (user) {
    return true;
  }

  return true;
};