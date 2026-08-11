import { computed, Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface'; 
import { GetCustomerDto, CreateCustomerDto } from '../../models/customer.interface'; 

@Injectable({
  providedIn: 'root' 
})
export class CustomerService {

  private apiUrl = 'http://localhost:5263/api/customers/';

  constructor(private http: HttpClient) { }

  private customerState = signal<GetCustomerDto[]>([]);

  public customers = this.customerState.asReadonly();

  public totalCustomers = computed(() => this.customerState().length);

  getAll(): Observable<ApiResponse<GetCustomerDto[]>> {
    return this.http.get<ApiResponse<GetCustomerDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.data) {
          this.customerState.set(response.data);
        }
        return response;
      })
    );
  }

  getById(id: number): Observable<ApiResponse<GetCustomerDto>> {
    return this.http.get<ApiResponse<GetCustomerDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        return response;
      })
    );
  }

  create(dto: CreateCustomerDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, dto).pipe(
      map(response => {
        if (response && response.data) {
          this.customerState.update(customers => [...customers, response.data]);
        }
        return response;
      })
    );
  }

  update(id: number, dto: CreateCustomerDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, dto).pipe(
      map(response => {
        if (response && response.data) {
          this.customerState.update(customers => customers.map(customer => customer.id === id ? response.data : customer));
        }
        return response;
      })
    );
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response && response.success) {
          this.customerState.update(customers => customers.filter(customer => customer.id !== id));
        }
        return response;
      })
    );
  }
}