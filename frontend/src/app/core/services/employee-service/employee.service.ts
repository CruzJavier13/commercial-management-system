import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface';
import { GetEmployeeDto, CreateEmployeeDto } from '../../models/employee.interface';

@Injectable({
  providedIn: 'root' // Instancia única global compartida en toda la aplicación
})
export class EmployeeService {

  private apiUrl = 'http://localhost:5263/api/employees';

  constructor(private http: HttpClient) { }

  private employeeState = signal<GetEmployeeDto[]>([]);
  public employees = this.employeeState.asReadonly();
  public totalEmployees = computed(() => this.employeeState().length);

  getAll(): Observable<ApiResponse<GetEmployeeDto[]>> {
    return this.http.get<ApiResponse<GetEmployeeDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.data) {
          this.employeeState.set(response.data);
        }
        return response;
      })
    );
  }

  getById(id: number): Observable<ApiResponse<GetEmployeeDto>> {
    return this.http.get<ApiResponse<GetEmployeeDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        return response;
      })
    );
  }

  create(dto: CreateEmployeeDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto).pipe(
      map(response => {
        if (response && response.data) {
          this.employeeState.update(employees => [...employees, response.data]);
        }
        return response;
      })
    );
  }


  update(id: number, dto: CreateEmployeeDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto).pipe(
      map(response => {
        if (response && response.data) {
          this.employeeState.update(employees => employees.map(employee => employee.id === id ? response.data : employee));
        }
        return response;
      })
    );
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response && response.success) {
          this.employeeState.update(employees => employees.filter(employee => employee.id !== id));
        }
        return response;
      })
    );
  }
}