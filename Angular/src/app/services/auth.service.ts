import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { UserInfoModel } from '../models/user_info.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  router = inject(Router);
  constructor() { }


  isAuthenticate(): boolean {
    try {
      const token = localStorage.getItem("access_token");
      if (token) {
        const decode: any = jwtDecode(token);
        return decode != null;
      }
      return false;

    } catch (err) {
      return false;
    }

  }
  logout(): void {
    const token = localStorage.getItem("access_token");

    if (token) {
      localStorage.removeItem("access_token");
      this.router.navigateByUrl("/login");
    }
  }
  getUser(): UserInfoModel | any {
    const token = localStorage.getItem("access_token");

    if (token) {
      const model: UserInfoModel = jwtDecode(token);
      return model;

    }

    return null;
  }
  isInRole(roleName: string): boolean {
    const token = localStorage.getItem("access_token");

    if (token) {
      const decode: any = jwtDecode(token);
      const clientRoles = (decode.resource_access.myclient?.roles as string[]) ?? [];
      const accountRoles = (decode.resource_access.account?.roles as string[]) ?? [];
      return clientRoles.concat(accountRoles).includes(roleName);
    }

    return false;
  }
}
