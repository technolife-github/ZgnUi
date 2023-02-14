import { Injectable } from '@angular/core';
import Swal, { SweetAlertIcon, SweetAlertPosition } from 'sweetalert2';

@Injectable({
  providedIn: 'root',
})
export class MessagerService {
  constructor() {}
  error(title: string, message: string = '') {
    Swal.fire({
      icon: 'error',
      title: title,
      text: message,
    });
  }
  success(title: string, message: string = '') {
    Swal.fire({
      icon: 'success',
      title: title,
      text: message,
    });
  }
  info(title: string, message: string = '') {
    Swal.fire({
      icon: 'info',
      title: title,
      text: message,
    });
  }
  warning(title: string, message: string = '') {
    Swal.fire({
      icon: 'error',
      title: title,
      text: message,
    });
  }
  any(message: string) {
    Swal.fire(message);
  }
  undefined(
    title: string,
    message: string = '',
    icon: SweetAlertIcon = 'question'
  ) {
    Swal.fire(title, message, icon);
  }
  simple( message:string,icon:SweetAlertIcon='success', position:SweetAlertPosition='top-end'){
    Swal.fire({
      position: position,
      icon: icon,
      title: message,
      showConfirmButton: false,
      timer: 1500
    })
  }
  custom(datas: any) {
    Swal.fire(datas);
  }
}
