import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { LayoutService } from 'src/app/services/layout.service';

@Component({
  selector: 'app-management',
  templateUrl: './management.component.html',
  styleUrls: ['./management.component.css']
})
export class ManagementComponent implements OnInit {
  loginType:string;
  fullName:string;
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
  toggleNav(){
    alert();
    document.querySelector('body')?.classList.toggle('nav-sm');
    document.querySelector('body')?.classList.toggle('nav-md');
  }

}
