import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  router = inject(Router);
  constructor() { }


  logout(): void {
    const token = localStorage.getItem("access_token");

    if (token) {
      localStorage.removeItem("access_token");
      this.router.navigateByUrl("/login");
    }
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
