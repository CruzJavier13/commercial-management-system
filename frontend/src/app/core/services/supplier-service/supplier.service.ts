import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface';
import { GetSupplierDto, CreateSupplierDto } from '../../models/supplier';

@Injectable({
  providedIn: 'root'
})
export class SupplierService {
  

  private apiUrl = 'https://localhost:7111/api/suppliers';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<GetSupplierDto[]>> {
    return this.http.get<ApiResponse<GetSupplierDto[]>>(this.apiUrl);
  }


  getById(id: number): Observable<ApiResponse<GetSupplierDto>> {
    return this.http.get<ApiResponse<GetSupplierDto>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateSupplierDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto);
  }

  update(id: number, dto: CreateSupplierDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}