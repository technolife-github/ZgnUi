import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { LayoutService } from 'src/app/services/layout.service';
declare var w2ui: any;
@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.css']
})
export class ManagementComponent implements OnInit {
  loginType:string;
  fullName: string;
  bodyView: string="nav-md"
  constructor(
    private layoutService :LayoutService,
    private ngxLoaderService:NgxUiLoaderService,
    private router:Router
    ) { }

  ngOnInit(): void {
    this.layoutService.pageType = 'home';
    this.loginType=localStorage.getItem("type")??""
    this.fullName=localStorage.getItem("user")??""
    this.ngxLoaderService.stop();
  }
  onLogout(){
    this.ngxLoaderService.start();
    localStorage.removeItem("token");
    this.router.navigate(['login']);

  }
  toggleNav() {
    if (this.bodyView == "nav-md") {
      this.bodyView = "nav-sm";
    }
    else{
      this.bodyView = "nav-md";
    }
    Object.keys(w2ui).forEach((key:string) => {
      w2ui[key].resize();
    });
  }

}
