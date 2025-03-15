import { Component, ElementRef, inject, OnInit, ViewChild } from '@angular/core';
import { RoleModel } from '../../models/role.model';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import Swal from 'sweetalert2';
import { AuthService } from '../../services/auth.service';
import { environment } from '../../../environments/environment';


@Component({
  selector: 'app-roles',
  imports: [FormsModule],
  templateUrl: './roles.component.html',
  styleUrl: './roles.component.css'
})
export class RolesComponent implements OnInit {

  roles: RoleModel[] = [];
  http = inject(HttpClient);
  name: string = "";
  description: string = "";
  authService = inject(AuthService);
  @ViewChild("addModalCloseBtn") addModalCloseBtn: ElementRef<HTMLButtonElement> | undefined;

  ngOnInit(): void {
    this.getAllRoles();
  }

  getAllRoles() {
    if (!this.authService.isInRole("RoleGetAll"))
      return;

    this.http.get<RoleModel[]>(`${environment.API_BASE_URL}/Roles/GetAll`).subscribe({
      next: (res: RoleModel[]) => {
        this.roles = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  delete(roleName: string): void {
    Swal.fire({
      title: `Are you sure delete for ${roleName} role?`,
      icon: "warning",
      showCloseButton: true,
      showConfirmButton: true,
      confirmButtonText: "Delete",
      cancelButtonText: "Close"

    }).then(res => {

      if (res.isConfirmed) {
        this.http.delete(`${environment.API_BASE_URL}/Roles/Delete/` + roleName).subscribe({
          next: (res: any) => {
            this.getAllRoles();
          },
          error: (err) => {
            console.log(err);
          },
        });

      }
    });

  }
  
  create() {

    if (!this.authService.isInRole("RoleCreate"))
      return;

    this.http.post(`${environment.API_BASE_URL}/Roles/Create`, { name: this.name, description: this.description })
      .subscribe({
        next: (res) => {
          this.name = "";
          this.description = "";
          this.addModalCloseBtn?.nativeElement.click();
          this.getAllRoles();
        },
        error: (err) => {
          Swal.fire({
            title: "Error",
            icon: "error",
            titleText: err.error.errorMessage
          })
        },
      });
  }
}
