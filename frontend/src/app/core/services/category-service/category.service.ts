import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetCategoryDto } from '../../models/category.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'https://localhost:7111/api/categories'; 

  constructor(private http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(categoryDto: GetCategoryDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, categoryDto);
  }

  update(id: number, categoryDto: GetCategoryDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, categoryDto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
