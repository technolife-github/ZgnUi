import { Injectable } from '@angular/core';
import {
  ActivatedRouteSnapshot,
  CanActivate,
  Router,
  RouterStateSnapshot,
  UrlTree,
} from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { MessagerService } from '../services/messager.service';

@Injectable({
  providedIn: 'root',
})
export class LoginGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private messager: MessagerService,
    private router: Router
  ) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):
    | Observable<boolean | UrlTree>
    | Promise<boolean | UrlTree>
    | boolean
    | UrlTree {
    if (localStorage.getItem('token') == null) {
      localStorage.removeItem('token');
      localStorage.removeItem('type');
      localStorage.removeItem('user');
      this.router.navigate(['/login']);
      this.messager.simple('Önce giiş yapmalısınız','error');
      return false;
    }
    var response = this.authService.isAuthenticated();
    response.subscribe(
      (result) => {
        if (result == false) {
          localStorage.removeItem('token');
          localStorage.removeItem('type');
          localStorage.removeItem('user');
          this.router.navigate(['/login']);
          this.messager.simple('Oturumunuz sona erdi.', 'warning');
        }
      },
      (error) => {
        localStorage.removeItem('token');
        localStorage.removeItem('type');
        localStorage.removeItem('user');
        this.router.navigate(['/admin/auth/login']);
        this.messager.simple('Giriş yapmalısınız', 'warning');
      }
    );
    return response;
  }
}
