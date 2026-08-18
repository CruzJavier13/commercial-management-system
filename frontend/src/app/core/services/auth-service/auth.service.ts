import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface'; 
import { UserSession } from '../../models/login.interface';

@Injectable({
  providedIn: 'root' 
})
export class AuthService {

  private apiUrl = 'http://localhost:5263/api/login/';

  constructor(private http: HttpClient) {

    const savedSession = localStorage.getItem('user_session');
    if (savedSession) {
      try {
        this.currentUser.set(JSON.parse(savedSession));
      } catch (e) {
        localStorage.clear();
      }
    }
  }

  public currentUser = signal<UserSession | null>(null);


  login(credentials: any): Observable<UserSession> {
    return this.http.post<ApiResponse<UserSession>>(this.apiUrl, credentials).pipe(
        map(response => {
        const session: UserSession = response.data; // Mapea el DTO de la API
        
        localStorage.setItem('access_token', session.token);
        localStorage.setItem('user_session', JSON.stringify(session));

        this.currentUser.set(session);
        return session;
        })
    );
  }

  logout(): void {
    localStorage.removeItem('access_token');
    localStorage.removeItem('user_session');


    this.currentUser.set(null);
  }

}