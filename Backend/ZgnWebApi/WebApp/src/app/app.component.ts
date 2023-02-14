import { Component } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ZgnUI';
  constructor(private ngxLoaderService:NgxUiLoaderService){
    this.ngxLoaderService.start();
  }
}
