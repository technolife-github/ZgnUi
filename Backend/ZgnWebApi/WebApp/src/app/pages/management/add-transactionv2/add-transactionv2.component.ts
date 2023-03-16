import { TransactionService } from './../../../services/transaction.service';

import { StationService } from './../../../services/station.service';
import { BlueBoticsService } from './../../../services/blue-botics.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SapGroup, BlueBoticsItem, Station, StationNode, SapItem, Transaction } from './../../../models/models';
import { MessagerService } from 'src/app/services/messager.service';
import { SapService } from './../../../services/sap.service';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-add-transactionv2',
  templateUrl: './add-transactionv2.component.html',
  styleUrls: ['./add-transactionv2.component.css']
})
export class AddTransactionv2Component implements OnInit {

  tabName='request'
  sapGroups:SapGroup[]=[]
  products:SapItem[]=[]
  selectedNode:StationNode|undefined
  selectedStationIndex:number
  stations:Station[]=[]
  group:string
  product:string
  description:string=""
  filterText:string=""
  constructor(
    private sapService:SapService,
    private messagerService:MessagerService,
    private stationService:StationService,
    private transactionService:TransactionService
    ) { }

  ngOnInit(): void {
    this.getGroups();
    this.getStations();
  }
  getGroups(){
    this.sapService.getGroups().subscribe(res=>{
      this.sapGroups=res.Data
    },error=>this.messagerService.error(error))
  }
  getProducts(){
    this.sapService.getAllByGroupName(this.group).subscribe(res=>{
      this.products=res.Data
    },error=>this.messagerService.error(error))
  }
  getStations(){
    this.stationService.GetAllByLoginUser().subscribe(res=>{
      this.stations=res.Data.filter(x=>x.Type=='İade Alma Noktası'||x.Type=='Bırakma Noktası')
    },error=>this.messagerService.error(error))
  }
  setTab(name:string){
    this.tabName=name;
  }
  setSelectedStationNode(item:StationNode){
    this.selectedNode=item
  }
  getGroupName(){
    return this.sapGroups.find(x=>x.Path==this.group)?.Name
  }
  getProductName(){
    return this.products.find(x=>x.MATNR==this.product)?.MAKTX
  }
  clearStationNode(){
    this.selectedNode=undefined
    console.log(this.selectedNode);
  }
  addTransaction(){
    if(this.group==undefined){
      this.messagerService.info("Lütfen ürün grubu seçiniz")
      return
    }
    if(this.product==undefined){
      this.messagerService.info("Lütfen ürün seçiniz")
      return
    }
    if(this.stations[this.selectedStationIndex]==undefined){
      this.messagerService.info("Lütfen istasyon seçiniz")
      return
    }
    if(this.selectedNode==undefined){
      this.messagerService.info("Lütfen "+(this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?'talep':'iade')+" noktası seçiniz")
      return
    }
    Swal.fire({
      title: '<strong>İşlem Detayları</strong>',
      icon: 'info',
      html:
        `<table style="text-align:left">
        <tr>
          <th style="width:140px">Ürün Grubu</th>
          <td>${this.getGroupName()}</td>
        </tr>
        <tr>
          <th>Ürün Adı</th>
          <td>${this.getProductName()}</td>
        </tr>
        <tr>
          <th>İşlem Tipi</th>
          <td>${this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?'Yeni Talep':'İade Talebi'}</td>
        </tr>
        <tr>
          <th>${this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?'Talep':'İade'} Noktası</th>
          <td>${this.selectedNode.NodeId}</td>
        </tr>
        <tr>
          <th>Açıklama</th>
          <td>${this.description}</td>
        </tr>
      </table>`,
      showCloseButton: true,
      showCancelButton: true,
      focusConfirm: false,
      confirmButtonText:
        'Onayla',
      cancelButtonText:
        'Vazgeç',

    }).then((result:any) => {
      if (result.isConfirmed) {
        this.saveTransaction();
      }
    })
  }
  saveTransaction(){
    var transaction:Transaction=Object.assign({
      TransactionType:this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?'TALEP':'İADE',
      Description:this.description,
      FromNode:this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?null:this.selectedNode.NodeId,
      ToNode:this.stations[this.selectedStationIndex].Type=='Bırakma Noktası'?this.selectedNode.NodeId:null,
      LocationName:this.stations[this.selectedStationIndex].Name,
      GroupCode:this.getGroupName(),
      SerialNumber:this.products.find(p=>p.MATNR==this.product).MATNR,
      ProductCode:this.products.find(p=>p.MATNR==this.product).MAKTX,
      Status:'Pending'

    })
    this.transactionService.add(transaction).subscribe(res=>{
      if(res.Success){
        this.messagerService.simple(res.Message,'success')
        this.selectedNode=undefined
        this.selectedStationIndex=undefined
        this.stations=undefined
        this.group=undefined
        this.product=undefined
        this.description=""
        window.location.reload()
      }else{

        this.messagerService.simple(res.Message,'error')
      }
    });
  }
  getFilterProduct(){
    var ps= this.products.filter(e=>e.MAKTX.toLowerCase().includes(this.filterText.toLowerCase())||e.MATNR.toLowerCase().includes(this.filterText.toLowerCase()));
    console.log(ps);
    return ps
  }


}
