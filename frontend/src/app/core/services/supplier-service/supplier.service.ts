import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface';
import { GetSupplierDto, CreateSupplierDto } from '../../models/supplier';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  

  private apiUrl = 'http://localhost:5263/api/suppliers';

  constructor(private http: HttpClient) { }

  private supplierState = signal<GetSupplierDto[]>([]);

  public suppliers = this.supplierState.asReadonly();
  
  public totalSuppliers = computed(() => this.supplierState().length);

  getAll(): Observable<ApiResponse<GetSupplierDto[]>> {
    return this.http.get<ApiResponse<GetSupplierDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.success) {
          this.supplierState.set(response.data);
        }
        return response;
      })
    );
  }


  getById(id: number): Observable<ApiResponse<GetSupplierDto>> {
    return this.http.get<ApiResponse<GetSupplierDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        return response;
      })
    );
  }

  create(dto: CreateSupplierDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto).pipe(
      map(response => {
        if (response && response.success) {
          this.supplierState.update(suppliers => [...suppliers, response.data]);
        }
        return response;
      })
    );
  }

  update(id: number, dto: CreateSupplierDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto).pipe(
      map(response => {
        if (response && response.success) {
          this.supplierState.update(suppliers => suppliers.map(s => s.id === id ? response.data : s));
        }
        return response;
      })
    );
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response && response.success) {
          this.supplierState.update(suppliers => suppliers.filter(s => s.id !== id));
        }
        return response;
      })
    );
  }
}