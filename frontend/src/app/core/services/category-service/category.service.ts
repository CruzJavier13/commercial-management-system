import { Injectable, Signal, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'; 
import { CreateCategoryDto, GetCategoryDto } from '../../models/category.interface';
import { ApiResponse } from '../../models/response.interface';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private apiUrl = 'http://localhost:5263/api/categories'; 

  constructor(private http: HttpClient) { }

  private categoryState = signal<GetCategoryDto[]>([]);

  public categories = this.categoryState.asReadonly();
  
  public totalCategories = computed(() => this.categoryState().length);

  getAll(): Observable<ApiResponse<GetCategoryDto[]>> {
    return this.http.get<ApiResponse<GetCategoryDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.data) {
          this.categoryState.set(response.data);
        }
        return response;
      })
    );
  }

  getById(id: number): Observable<ApiResponse<GetCategoryDto>> {
    return this.http.get<ApiResponse<GetCategoryDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response && response.data) {
          this.categoryState.set([...this.categoryState(), response.data]);
        }
        return response;
      })
    );
  }

  create(categoryDto: CreateCategoryDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, categoryDto).pipe(
      map(response => {
        if (response && response.data) {
          this.categoryState.set([...this.categoryState(), response.data]);
        }
        return response;
      })
    );
  }

  update(id: number, dto: CreateCategoryDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto).pipe(
      map(response => {
        if (response && response.data) {
          const updatedCategories = this.categoryState().map(cat => cat.id === id ? { ...cat, ...response.data } : cat);
          this.categoryState.set(updatedCategories);
        }
        return response;
      })
    );
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        this.categoryState.set(this.categoryState().filter(cat => cat.id !== id));
        return response;
      })
    );
  }
}
