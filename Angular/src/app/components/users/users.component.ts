import { HttpClient } from '@angular/common/http';
import { Component, Inject, inject, OnInit } from '@angular/core';
import { UserModel } from '../../models/user.model';
import { Constants } from '../../../constantsEnv';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-users',
  imports: [],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  const = inject(Constants);
  users: UserModel[] = [];
  authService = inject(AuthService);
  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.userGetAll();
  }

  userGetAll() {
    this.http.get<UserModel[]>(`${this.const.getApiBaseUrl()}/Users/GetAll`).subscribe({
      next: (res: UserModel[]) => {
        this.users = res;
        console.log(res);
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  delete(userId: string) {
    Swal.fire({
      title: "Are you sure delete?",
      icon: "warning",
      showCloseButton: true,
      showConfirmButton: true,
      confirmButtonText: "Delete",
      cancelButtonText: "Close"

    }).then(res => {

      if (res.isConfirmed) {
        this.http.delete(`${this.const.getApiBaseUrl()}/Users/Delete/` + userId).subscribe({
          next: (res: any) => {
            console.log(res);
            this.userGetAll();
          },
          error: (err) => {
            console.log(err);
          },
        });

      }
    });

  }
}
