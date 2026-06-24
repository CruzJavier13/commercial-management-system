import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ApiResponse } from '../../models/response.interface'; 
import { GetInvoiceDto, CreateInvoiceDto } from '../../models/billing.interface'; // Asegúrate de tener estos DTOs definidos

@Injectable({
  providedIn: 'root'
})
export class BillingService {


  private apiUrl = 'http://localhost:5263/api/billing/invoices/';

  constructor(private http: HttpClient) { }

  getInvoiceHistory(): Observable<ApiResponse<GetInvoiceDto[]>> {
    return this.http.get<ApiResponse<GetInvoiceDto[]>>(this.apiUrl);
  }

  createInvoice(invoiceDto: CreateInvoiceDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, invoiceDto);
  }
}