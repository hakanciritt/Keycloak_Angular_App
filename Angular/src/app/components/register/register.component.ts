import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterModel } from '../../models/register.model';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import Swal from 'sweetalert2';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-register',
  imports: [FormsModule,RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css',
})
export class RegisterComponent {
  registerModel: RegisterModel = new RegisterModel();
  http = inject(HttpClient);
  router = inject(Router);

  register() {
    
    this.http
      .post(`${environment.API_BASE_URL}/Auth/Register`, this.registerModel)
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
