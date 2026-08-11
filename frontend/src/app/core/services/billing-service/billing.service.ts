import { Injectable, computed, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators'; 
import { ApiResponse } from '../../models/response.interface'; 
import { GetInvoiceDto, CreateInvoiceDto } from '../../models/billing.interface'; // Asegúrate de tener estos DTOs definidos

@Injectable({
  providedIn: 'root'
})
export class BillingService {


  private apiUrl = 'http://localhost:5263/api/billing/invoices/';

  constructor(private http: HttpClient) { }

  private billingState = signal<GetInvoiceDto[]>([]);

  public invoices = this.billingState.asReadonly();

  public totalInvoices = computed(() => this.billingState().length);

  getInvoiceHistory(): Observable<ApiResponse<GetInvoiceDto[]>> {
    return this.http.get<ApiResponse<GetInvoiceDto[]>>(this.apiUrl).pipe(
      map(response => {
        if (response && response.data) {
          this.billingState.set(response.data);
        }
        return response;
      })
    );
  }

  createInvoice(invoiceDto: CreateInvoiceDto): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.apiUrl, invoiceDto).pipe(
      map(response => {
        this.getInvoiceHistory().subscribe();
        return response;
      })
    );
  }
}