import { inject } from '@angular/core';
import { CanActivateChildFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const roleGuard: CanActivateChildFn = (childRoute, state) => {

  const roleName: any = childRoute.data["role"] ?? "";

  const authService = inject(AuthService);
  const router = inject(Router);

  const checkRole: boolean = authService.isInRole(roleName);

  if (!checkRole)
    return router.navigateByUrl("/home")

  return true;
};
