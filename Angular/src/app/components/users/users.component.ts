import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Inject, inject, OnInit, ViewChild } from '@angular/core';
import { UserModel } from '../../models/user.model';
import { Constants } from '../../../constantsEnv';
import { AuthService } from '../../services/auth.service';
import Swal from 'sweetalert2'
import { RoleModel } from '../../models/role.model';
import { CustomUserRoleModel } from '../../models/custom_user_role.model';
import { UserRoleModel } from '../../models/user_role.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-users',
  imports: [FormsModule],
  templateUrl: './users.component.html',
  styleUrl: './users.component.css',
})
export class UsersComponent implements OnInit {
  const = inject(Constants);
  users: UserModel[] = [];
  userRoles: CustomUserRoleModel[] = [];
  userOldSelectedRoles : UserRoleModel[] = [];

  authService = inject(AuthService);
  @ViewChild("roleModalCloseBtn") roleModalCloseBtn : ElementRef<HTMLButtonElement> | undefined;
  constructor(private http: HttpClient) {

  }

  ngOnInit(): void {
    this.userGetAll();
  }

  userGetAll() {
    this.http.get<UserModel[]>(`${this.const.getApiBaseUrl()}/Users/GetAll`).subscribe({
      next: (res: UserModel[]) => {
        this.users = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getUserRoles(userId: string) {
    //get all roles 
    this.http.get<RoleModel[]>(`${this.const.getApiBaseUrl()}/Roles/GetAll`).subscribe({
      next: (roles: RoleModel[]) => {
        this.userRoles = [];
        roles.forEach((value, index) => {
          let userRoleModel = new CustomUserRoleModel();
          userRoleModel.id = value.id;
          userRoleModel.isSelect = false;
          userRoleModel.name = value.name;

          this.userRoles.push(userRoleModel);
        });

        //user roles
        this.http.get<UserRoleModel[]>(`${this.const.getApiBaseUrl()}/UserRoles/GetAllUserRoles/${userId}`)
          .subscribe({
            next: (userRoles: UserRoleModel[]) => {
              this.userOldSelectedRoles = userRoles;
              
              userRoles.forEach(value => {
                let existsRole = this.userRoles.findIndex(d => d.name == value.name);
                if (existsRole) {
                  this.userRoles[existsRole].isSelect = true;
                }
              });

            },
            error: (err) => {
              console.log(err);
            },
          });
      },
      error: (err) => {
        console.log(err);
      },
    });
  }
  saveRoles() {
    
    this.roleModalCloseBtn?.nativeElement.click();
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
