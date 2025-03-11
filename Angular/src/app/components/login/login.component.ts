import { HttpClient } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { LoginModel } from '../../models/login.model';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { Constants } from '../../../constantsEnv';

@Component({
  selector: 'app-login',
  imports: [RouterLink, FormsModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  loginModel: LoginModel = new LoginModel();
  const = inject(Constants);

  constructor(protected httpClient: HttpClient, private router: Router) {

  }

  login() {
    this.httpClient.post(`${this.const.getApiBaseUrl()}/Auth/Login`, this.loginModel).subscribe({
      next: (res: any) => {
        const accessToken = localStorage.getItem("access_token");
        if (!accessToken) {
          localStorage.setItem("access_token", res.access_token);
        }
        this.router.navigateByUrl("/home");

      },
      error: (err) => {
        Swal.fire({
          title: err.error.error_description,
          icon: "error",
        });
        console.log(err);

      }
    }
    )
  }
}
