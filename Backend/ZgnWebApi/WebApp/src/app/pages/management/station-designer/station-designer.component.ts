import { Station, StationNode } from './../../../models/models';
import { MessagerService } from 'src/app/services/messager.service';
import { StationService } from './../../../services/station.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-station-designer',
  templateUrl: './station-designer.component.html',
  styleUrls: ['./station-designer.component.css']
})
export class StationDesignerComponent implements OnInit {
  id:number
  station:Station
  selectedNode:StationNode
  selectedNodeIndex:number=-1
  constructor(
    private activatedRoute:ActivatedRoute,
    private router:Router,
    private stationService:StationService,
    private messagerService:MessagerService
    ) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(param=>{
      if(param['id']==undefined) this.router.navigateByUrl('/');
      this.id=param['id']
      this.getStation()
    })
  }
  getStation(){
    this.stationService.getById(this.id).subscribe(res=>{
      this.station=res.Data
    },error=>{
      console.log(error);
      this.messagerService.error("Hata","Bir hata ile karşılaşıldı.")
    })
  }
  update(){
    this.stationService.update(this.station).subscribe(res=>{
      if(res.Success){
        this.messagerService.success("Başarılı",res.Message)
      }else{
        this.messagerService.error("Başarısız",res.Message)
      }
    },error=>{
      console.log(error);
      this.messagerService.error("Hata","Bir hata ile karşılaşıldı.")
    })
  }
  goBack(){
    this.router.navigateByUrl('/stations')
  }
}
