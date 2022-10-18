export interface LoginDto{
  UserName:string;
  Password:string;
}
export interface TokenResponse{
  Token:string;
  Type:string;
  FullName:string;
  Expiration:Date;
}

export interface SingleResult{
  success:string;
  message:string;
}

export interface PayloadResult<T>{
  Error:string;
  RetCode:T;
  Payload:string;
}
