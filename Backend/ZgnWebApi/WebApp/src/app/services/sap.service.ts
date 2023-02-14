import { DataResult, SapGroup, SapItem } from './../models/models';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class SapService {
  apiUrl:string=environment.apiUrl
  constructor(private httpClient:HttpClient) {
   }

   getGroups():Observable<DataResult<SapGroup[]>>{
    return this.httpClient.get<DataResult<SapGroup[]>>(`${this.apiUrl}Sap/GetGroups`);
  }
  getAllByGroupName(groupName:string):Observable<DataResult<SapItem[]>>{
    return this.httpClient.get<DataResult<SapItem[]>>(`${this.apiUrl}Sap/getAllByGroupName/${groupName}`);
  }

}
