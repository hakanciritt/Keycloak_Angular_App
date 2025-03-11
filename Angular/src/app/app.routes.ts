import { Routes } from '@angular/router';
import { LoginComponent } from './components/login/login.component';
import { LayoutsComponent } from './components/layouts/layouts.component';
import { RegisterComponent } from './components/register/register.component';
import { HomeComponent } from './components/home/home.component';
import { RolesComponent } from './components/roles/roles.component';
import { UsersComponent } from './components/users/users.component';
import { authGuard } from './guards/auth.guard';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'home', component: LayoutsComponent },
    {
        path: '', component: LayoutsComponent,
        canActivateChild: [authGuard, roleGuard],
        children: [
            { path: '', component: HomeComponent },
            { path: 'users', component: UsersComponent, data: { role: "UserGetAll" } },
            { path: 'roles', component: RolesComponent, data: { role: "RoleGetAll" } },
        ]
    },
];
