import { ActivatedRoute, Router } from '@angular/router';
import { TransactionService } from './../../../services/transaction.service';

import { StationService } from './../../../services/station.service';
import { BlueBoticsService } from './../../../services/blue-botics.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { SapGroup, BlueBoticsItem, Station, StationNode, SapItem, Transaction } from './../../../models/models';
import { MessagerService } from 'src/app/services/messager.service';
import { SapService } from './../../../services/sap.service';
import { Component, OnInit } from '@angular/core';
import Swal from 'sweetalert2';
import { param } from 'jquery';

@Component({
  selector: 'app-commit-transaction',
  templateUrl: './commit-transaction.component.html',
  styleUrls: ['./commit-transaction.component.css']
})
export class CommitTransactionComponent implements OnInit {
  transaction:Transaction
  selectedNode:StationNode|undefined
  selectedStationIndex:number
  stations:Station[]=[]
  product:string
  description:string=""
  constructor(
    private messagerService:MessagerService,
    private stationService:StationService,
    private transactionService:TransactionService,
    private activatedRoute:ActivatedRoute,
    private router:Router
    ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param=>{
      this.getTransaction(param['id'])
    })
    this.getStations();
  }
  getTransaction(id:number){
    this.transactionService.getById(id).subscribe(res=>{
      this.transaction=res.Data
    })
  }
  getStations(){
    this.stationService.GetAllByLoginUser().subscribe(res=>{
      let type=this.transaction.TransactionType=='İADE'?'İade Teslim Noktası':'Alma Noktası'
      this.stations=res.Data.filter(x=>x.Type==type)
    },error=>this.messagerService.error(error))
  }
  setSelectedStationNode(item:StationNode){
    this.selectedNode=item
  }
  clearStationNode(){
    this.selectedNode=undefined
    console.log(this.selectedNode);
  }
  addTransaction(){
    if(this.stations[this.selectedStationIndex]==undefined){
      this.messagerService.info("Lütfen istasyon seçiniz")
      return
    }
    if(this.selectedNode==undefined){
      this.messagerService.info("Lütfen "+(this.transaction.TransactionType=='İADE'?'teslimat':'bırakma')+" noktası seçiniz")
      return
    }
    Swal.fire({
      title: '<strong>İşlem Detayları</strong>',
      icon: 'info',
      html:
        `<table style="text-align:left">
        <tr>
          <th>${this.transaction.TransactionType=='İADE'?'Teslimat':'Bırakma'} Noktası</th>
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
    if(this.description!="")
      this.transaction.Description+=" Cevap: "+this.description;
      if(this.transaction.TransactionType=='İADE'){
        this.transaction.ToNode=this.selectedNode.NodeId
      }else{
        this.transaction.FromNode=this.selectedNode.NodeId
      }
    this.transactionService.start(this.transaction).subscribe(res=>{
      if(res.Success){
        this.messagerService.simple(res.Message,'success')
        this.selectedNode=undefined
        this.selectedStationIndex=undefined
        this.stations=undefined
        this.product=undefined
        this.description=""
        this.router.navigateByUrl('/pending-transactions')
      }else{

        this.messagerService.simple(res.Message,'error')
      }
    });
  }
}
