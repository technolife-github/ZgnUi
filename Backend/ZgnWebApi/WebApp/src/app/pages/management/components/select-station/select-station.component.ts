import { BlueBoticsItem, Station, StationNode } from './../../../../models/models';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-select-station',
  templateUrl: './select-station.component.html',
  styleUrls: ['./select-station.component.css']
})
export class SelectStationComponent implements OnInit {

  @Input() station:Station
  selectedNode:StationNode;
  @Output() selectedNodeEmitter=new EventEmitter<StationNode>();
  constructor() { }

  ngOnInit(): void {
  }
  getStyle(i:number){
    const element=Array.from(document.getElementsByClassName('layout-content') as HTMLCollectionOf<HTMLElement>)
    element[0].style.width=(this.station.ColumnLen*104)+"px";
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=[].constructor(this.station.ColumnLen??0*this.station.RowLen??0).length-parseInt(""+(i/this.station.ColumnLen))
    let result= `--zindx: ${x}; --zindy: ${y}; `
    x=parseInt(""+(i%this.station.ColumnLen))
    y=parseInt(""+(i/this.station.ColumnLen))
    result=this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).length>0?result:"opacity:0;"
    return result
  }
  getClassValue(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    let result=this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).length>0?'':' empty'
    if(this.selectedNode!=null)
      result+=this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y&&n.NodeId==this.selectedNode.NodeId).length>0?' active':''
    return result
  }
  getTitle(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    let result=""
    this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).forEach(n => {
      result=n.NodeId
    });
    return result
  }
  getName(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    let result='Boş'
    this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).forEach(n=>{
      var arr=n.NodeId?.split('_')??[]
      if(arr.length>0)
      result=(arr.length>1)?`${arr[0]}_${arr[arr.length-1]}`:n.NodeId
    })
    return result
  }
  setSelected(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))

    this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).forEach(n=>{
      this.selectedNode=n
      this.selectedNodeEmitter.emit(n)
    })
  }
}
