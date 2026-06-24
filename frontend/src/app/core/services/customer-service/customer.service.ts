import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface'; 
import { GetCustomerDto, CreateCustomerDto } from '../../models/customer.interface'; 

@Injectable({
  providedIn: 'root' 
})
export class CustomerService {

  private apiUrl = 'http://localhost:5263/api/customers/';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<GetCustomerDto[]>> {
    return this.http.get<ApiResponse<GetCustomerDto[]>>(this.apiUrl);
  }

  getById(id: number): Observable<ApiResponse<GetCustomerDto>> {
    return this.http.get<ApiResponse<GetCustomerDto>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateCustomerDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto);
  }

  update(id: number, dto: CreateCustomerDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}