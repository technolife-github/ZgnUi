import {
  DataResult,
  SapGroup,
  SapItem,
  Station,
  SingleResult,
} from './../models/models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root',
})
export class StationService {
  apiUrl: string = environment.apiUrl;
  constructor(private httpClient: HttpClient) {}

  getAll(): Observable<DataResult<Station[]>> {
    return this.httpClient.get<DataResult<Station[]>>(
      `${this.apiUrl}Stations/GetAll`
    );
  }
  GetAllByLoginUser(): Observable<DataResult<Station[]>> {
    return this.httpClient.get<DataResult<Station[]>>(
      `${this.apiUrl}Stations/GetAllByLoginUser`
    );
  }
  getById(id: number): Observable<DataResult<Station>> {
    return this.httpClient.get<DataResult<Station>>(
      `${this.apiUrl}Stations/GetById/${id}`
    );
  }
  update(station: Station): Observable<SingleResult> {
    return this.httpClient.put<SingleResult>(
      `${this.apiUrl}Stations/Update`,
      station
    );
  }
}
