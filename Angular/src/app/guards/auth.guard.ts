import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  
  const authService = inject(AuthService);
  const isAuthenticate : boolean = authService.isAuthenticate();

  if (!isAuthenticate) {
    const router = inject(Router);
    router.navigateByUrl('/login');
    return false;
  }

  return true;
};
