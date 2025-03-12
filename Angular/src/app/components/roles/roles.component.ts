import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { RoleModel } from '../../models/role.model';
import { HttpClient } from '@angular/common/http';
import { Constants } from '../../../constantsEnv';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { AuthService } from '../../services/auth.service';


@Component({
  selector: 'app-roles',
  imports: [FormsModule],
  templateUrl: './roles.component.html',
  styleUrl: './roles.component.css'
})
export class RolesComponent implements OnInit {

  roles: RoleModel[] = [];
  const = inject(Constants);
  http = inject(HttpClient);
  name: string = "";
  description: string = "";
  authService = inject(AuthService);
  @ViewChild("addModalCloseBtn") addModalCloseBtn : ElementRef<HTMLButtonElement> | undefined;

  ngOnInit(): void {
    this.getAllRoles();
  }

  getAllRoles() {
    if(!this.authService.isInRole("RoleGetAll"))
      return;

    this.http.get<RoleModel[]>(`${this.const.getApiBaseUrl()}/Roles/GetAll`).subscribe({
      next: (res: RoleModel[]) => {
        console.log(res);

        this.roles = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  create() {

    if(!this.authService.isInRole("RoleCreate"))
      return;
    
    this.http.post(`${this.const.getApiBaseUrl()}/Roles/Create`, { name: this.name, description: this.description })
      .subscribe({
        next: (res) => {
          this.name = "";
          this.description = "";
          this.addModalCloseBtn?.nativeElement.click();
          this.getAllRoles();
        },
        error: (err) => {
          Swal.fire({
            title : "Error",
            icon : "error",
            titleText : err.error.errorMessage
          })
        },
      });
  }


}
