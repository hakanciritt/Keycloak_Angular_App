import { HttpErrorResponse, HttpInterceptorFn, HttpStatusCode } from '@angular/common/http';
import { catchError, throwError } from 'rxjs';
import Swal from 'sweetalert2';

export const exceptionInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      return throwError(() => {

        if (error.status == HttpStatusCode.Forbidden) {
          Swal.fire({
            title: "Error",
            icon: "error",
            text: "you dont have access permission"
          });
        }
        else if (error.status == HttpStatusCode.Unauthorized) {
          Swal.fire({
            title: "Error",
            icon: "error",
            text: "you have to login"
          });
        } else {
          Swal.fire({
            title: "Error",
            icon: "error",
            text: "something went a wrong"
          });
        }

        return error;
      });
    })
  );
};
