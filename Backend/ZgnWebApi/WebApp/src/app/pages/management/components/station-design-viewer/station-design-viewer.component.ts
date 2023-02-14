import { Station } from './../../../../models/models';
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-station-design-viewer',
  templateUrl: './station-design-viewer.component.html',
  styleUrls: ['./station-design-viewer.component.css']
})
export class StationDesignViewerComponent implements OnInit {
  @Input() station:Station
  constructor() { }

  ngOnInit(): void {
  }
  getStyle(i:number){
    const element=Array.from(document.getElementsByClassName('layout-content') as HTMLCollectionOf<HTMLElement>)
    element[0].style.width=(this.station.ColumnLen*104)+"px";
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=[].constructor(this.station.ColumnLen??0*this.station.RowLen??0).length-parseInt(""+(i/this.station.ColumnLen))
    return `--zindx: ${x}; --zindy: ${y}`
  }
  getName(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    let result='Boş'
    this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).forEach(n=>{
      result=n.NodeId
    })
    return result
  }
  getClassValue(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    let result=this.station.StationNodes.filter(n=>n.ColPosition-1==x&&n.RowPosition-1==y).length>0?'':' empty'
    return result
  }
  getLocation(i:number){
    let x=parseInt(""+(i%this.station.ColumnLen))
    let y=parseInt(""+(i/this.station.ColumnLen))
    return `Y-${x+1}_D-${y+1}`
  }
}
