import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterModel } from '../../models/register.model';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { Constants } from '../../../constantsEnv';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerModel: RegisterModel = new RegisterModel();
  http = inject(HttpClient);
  router = inject(Router);
  const  = inject(Constants);

  register() {
    
    this.http
      .post(`${this.const.getApiBaseUrl()}/Auth/Register`, this.registerModel)
      .subscribe({
        next: (res: any) => {
          console.log(res);
          this.router.navigateByUrl('/login');
        },
        error: (err) => {
          Swal.fire({
            title: err.error.errorMessage,
            icon: "error",
          });
          console.log(err);
        },
      });
  }
}
