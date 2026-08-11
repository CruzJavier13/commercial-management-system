import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { GetProductDto, CreateProductDto } from '../../models/product.interface';
import { ApiResponse } from '../../models/response.interface';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'http://localhost:5263/api/products';

  constructor(private http: HttpClient) { }

  private productState = signal<GetProductDto[]>([]);

  public products = this.productState.asReadonly();
  
  public totalProducts = computed(() => this.productState().length);

  getAll(): Observable<ApiResponse<GetProductDto[]>> {
    return this.http.get<ApiResponse<GetProductDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.data) {
          this.productState.set(response.data);
        }
        return response;
      })
    );
  }

  getById(id: number): Observable<ApiResponse<GetProductDto>> {
    return this.http.get<ApiResponse<GetProductDto>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        if (response && response.data) {
          this.productState.set([...this.productState(), response.data]);
        }
        return response;
      })
    );
  }

  create(categoryDto: CreateProductDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, categoryDto).pipe(
      map(response => {
        if (response && response.data) {
          this.productState.set([...this.productState(), response.data]);
        }
        return response;
      })
    );
  }

  update(id: number, categoryDto: GetProductDto): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(`${this.apiUrl}/${id}`, categoryDto).pipe(
      map(response => {
        if (response && response.data) {
          const updatedProducts = this.productState().map(product => product.id === id ? { ...product, ...response.data } : product);
          this.productState.set(updatedProducts);
        }
        return response;
      })
    );
  }

  delete(id: number): Observable<ApiResponse<any>> {
    return this.http.delete<ApiResponse<any>>(`${this.apiUrl}/${id}`).pipe(
      map(response => {
        this.productState.set(this.productState().filter(product => product.id !== id));
        return response;
      })
    );  
  }
}
