export interface LoginDto {
  UserName: string;
  Password: string;
}
export interface TokenResponse {
  Token: string;
  Type: string;
  FullName: string;
  Expiration: Date;
}

export interface SingleResult {
  Success: boolean;
  Message: string;
}
export interface DataResult<T> extends SingleResult {
  Data: T;
}

export interface PayloadResult<T> {
  Error: string;
  RetCode: T;
  Payload: string;
}
export interface SapGroup {
  Name: string;
  Path: string;
}
export interface Transaction {
  Id: number;
  TransactionType: string;
  Status: string;
  FromNode: string;
  ToNode: string;
  ProcessId: string;
  GroupCode: string;
  ProductId: number;
  ProductCode: string;
  SerialNumber: string;
  LocationName: string;
  Description: string;
  StartDate: string;
  EndDate: string;
}
export interface SapItem {
  MATNR: string;
  MAKTX: string;
}
export interface BlueBoticsItem {
  SymbolId: string;
  Name: string;
}
export interface OptionDataModel {
  id: any;
  text: any;
}
export interface StationNode {
  Id: number;
  StationId: number;
  NodeId: string;
  ColPosition:number;
  RowPosition:number;
}
export interface StationGroupCode {
  Id: number;
  StationId: number;
  GroupCode: string;
}
export interface Station {
  Id: number;
  Name: string;
  Type: string;
  ColumnLen:number;
  RowLen:number;
  Nodes: OptionDataModel[];
  GroupCodes: OptionDataModel[];
  StationNodes: StationNode[];
  StationGroupCodes:StationGroupCode[];
}
export interface Transaction{
  Id:number
  TransactionType:string//TALEP,IADE
  Status:string//Pending,Ready,End
  FromNode:string
  ToNode:string
  ProcessId:string
  GroupCode:string
  ProductId:number
  ProductCode:string
  SerialNumber:string
  LocationName:string
  Description:string
  StartDate:string
  EndDate:string
}
