import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {

  const token = localStorage.getItem("access_token") ?? "";

  const reqCopy = req.clone({
    headers: req.headers.set('Authorization', "Bearer " + token),
  });

  return next(reqCopy);
};
