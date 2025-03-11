import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';

export const authGuard: CanActivateChildFn = (childRoute, state) => {
  const token = localStorage.getItem('access_token');
  if (token == null) {
    const router = inject(Router);
    router.navigateByUrl('/login');
    return false;
  }

  return true;
};
