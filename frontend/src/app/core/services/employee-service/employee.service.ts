import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface';
import { GetEmployeeDto, CreateEmployeeDto } from '../../models/employee.interface';

@Injectable({
  providedIn: 'root' // Instancia única global compartida en toda la aplicación
})
export class EmployeeService {

  private apiUrl = 'http://localhost:5263/api/employees';

  constructor(private http: HttpClient) { }

  getAll(): Observable<ApiResponse<GetEmployeeDto[]>> {
    return this.http.get<ApiResponse<GetEmployeeDto[]>>(this.apiUrl);
  }

  getById(id: number): Observable<ApiResponse<GetEmployeeDto>> {
    return this.http.get<ApiResponse<GetEmployeeDto>>(`${this.apiUrl}/${id}`);
  }

  create(dto: CreateEmployeeDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto);
  }


  update(id: number, dto: CreateEmployeeDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto);
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`);
  }
}