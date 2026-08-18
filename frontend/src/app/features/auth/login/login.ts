import { Component, inject } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../core/services/auth-service/auth.service';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule],
  templateUrl: './login.html',
  styleUrl: './login.css',
})
export class Login {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  // Propiedades para controlar el estado de la pantalla
  public loginForm: FormGroup;
  public errorMessage: string | null = null;
  public isLoading: boolean = false;

  constructor() {
    this.loginForm = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(3)]]
    });
  }

  onSubmit(): void {
    if (this.loginForm.invalid) {
      this.loginForm.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.errorMessage = null;

    const credentials = this.loginForm.value;


    this.authService.login(credentials).subscribe({
      next: (session) => {
        this.isLoading = false;
        console.log('Login exitoso, bienvenido:', session.nombre);
        
        this.router.navigate(['/dashboard']); 
      },
      error: (err) => {
        this.isLoading = false;
        this.errorMessage = err.error?.message || 'Ocurrió un error al intentar iniciar sesión.';
        console.error('Error en el login:', err);
      }
    });
    //this.router.navigate(['/dashboard']);
  }
}
