import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateCategoryDto, GetCategoryDto } from '../../models/category.interface';
import { ApiResponse } from '../../models/response.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'http://localhost:5263/api/categories'; 

  constructor(private http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(categoryDto: CreateCategoryDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, categoryDto);
  }
  

  update(id: number, dto: CreateCategoryDto): Observable<ApiResponse<any>> {
  return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto);
}

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
