import { SingleResult, Transaction, DataResult } from './../models/models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class TransactionService {
  apiUrl: string = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  add(transaction:Transaction): Observable<SingleResult> {
    return this.httpClient.post<SingleResult>(
      `${this.apiUrl}Transactions/Add`,transaction
    );
  }
  getById(id:number): Observable<DataResult<Transaction>> {
    return this.httpClient.get<DataResult<Transaction>>(
      `${this.apiUrl}Transactions/GetById/${id}`);
  }
  start(transaction:Transaction): Observable<SingleResult> {
    return this.httpClient.post<SingleResult>(
      `${this.apiUrl}Transactions/Start`,transaction
    );
  }
}
