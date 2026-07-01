import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GetProductDto, CreateProductDto } from '../../models/product.interface';

@Injectable({
  providedIn: 'root',
})
export class ProductService {
  private apiUrl = 'http://localhost:5263/api/products';

  constructor(private http: HttpClient) { }

  getAll(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }

  getById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }

  create(categoryDto: CreateProductDto): Observable<any> {
    return this.http.post<any>(this.apiUrl, categoryDto);
  }

  update(id: number, categoryDto: GetProductDto): Observable<any> {
    return this.http.put<any>(`${this.apiUrl}/${id}`, categoryDto);
  }

  delete(id: number): Observable<any> {
    return this.http.delete<any>(`${this.apiUrl}/${id}`);
  }
}
