import { TokenResponse } from './../models/models';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginDto } from '../models/models';
import { environment } from 'src/environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  apiUrl:string=environment.apiUrl
  constructor(private httpClient:HttpClient) {
   }

  login(loginDto:LoginDto):Observable<TokenResponse> {
    return this.httpClient.post<TokenResponse>(`${this.apiUrl}auth/login`, loginDto);
  }
  isAuthenticated():Observable<boolean>{
    return this.httpClient.get<boolean>(this.apiUrl+"auth/isAuthenticated");
  }

}
