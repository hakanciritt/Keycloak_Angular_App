import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-layouts',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './layouts.component.html',
  styleUrl: './layouts.component.css'
})
export class LayoutsComponent {


  authService = inject(AuthService);
  constructor(){
    this.authService.isInRole("");
  }
}
