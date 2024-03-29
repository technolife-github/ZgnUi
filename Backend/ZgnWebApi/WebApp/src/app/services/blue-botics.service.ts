import { DataResult, BlueBoticsItem } from './../models/models';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment.prod';
import { PayloadResult } from '../models/models';

@Injectable({
  providedIn: 'root'
})
export class BlueBoticsService {
  apiUrl:string=environment.apiUrl
  constructor(private httpClient:HttpClient) {
   }

   cancelMission(missionId:any):Observable<PayloadResult<any>>{
     return this.httpClient.get<PayloadResult<any>>(`${this.apiUrl}BlueBotics/CancelMissionById/${missionId}`);
   }

   cancelMissions():Observable<PayloadResult<any>>{
     return this.httpClient.get<PayloadResult<any>>(`${this.apiUrl}BlueBotics/CancelMissions`);
   }
  monitorCancelMission(missionId: any): Observable<PayloadResult<any>>{
    return this.httpClient.get<PayloadResult<any>>(`${this.apiUrl}BlueBotics/MonitorCancelMissionById/${missionId}`);
}
   getAllDropNodesByLoginUser():Observable<DataResult<BlueBoticsItem[]>>{
    return this.httpClient.get<DataResult<BlueBoticsItem[]>>(`${this.apiUrl}BlueBotics/GetAllDropNodesByLoginUser`);
   }
}
