import { AuthService } from './../../services/auth.service';
import { LoginDto } from './../../models/models';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { LayoutService } from './../../services/layout.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { MessagerService } from 'src/app/services/messager.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  loginForm:FormGroup
  constructor(
    private layoutService :LayoutService,
    private formBuilder:FormBuilder,
    private ngxLoaderService:NgxUiLoaderService,
    private authService:AuthService,
    private router:Router,
    private messager:MessagerService
    ) { }

  ngOnInit(): void {
    this.layoutService.pageType = 'login';
    this.createForm();
    this.ngxLoaderService.stop();
  }
  createForm(){
    this.loginForm = this.formBuilder.group({
      UserName:['',Validators.required],
      Password:['',Validators.required]
    })
  }
  onSubmit(){
    console.log(this.router);
    if(!this.loginForm.valid){
      this.messager.simple("Lütfen giriş bilgilerini kontrol ediniz.","warning");
      return;
    }
    this.ngxLoaderService.start();
    var loginDto:LoginDto=Object.assign({},this.loginForm.value);
    this.authService.login(loginDto).subscribe(response=>{
      this.ngxLoaderService.stop();
      localStorage.setItem("token",response.Token);
      localStorage.setItem("type",response.Type);
      localStorage.setItem("user",response.FullName);
      this.messager.simple("Giriş Başarılı");
        this.router.navigate(['/']);
    },

    (error:any)=>{
      this.messager.error('Hata',error.error.Message);
      this.ngxLoaderService.stop();
    });
  }

}
