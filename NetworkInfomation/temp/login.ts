// login.component.ts
import { Component } from '@angular/core';
import { AuthService } from './auth.service';

@Component({
  selector: 'app-login',
  template: `
    <button (click)="login()">Login</button>
  `
})
export class LoginComponent {
  constructor(private authService: AuthService) {}

  login(): void {
    // Call your authentication service to perform login
    // and store the token in AuthService
    const token = 'your_generated_token';
    this.authService.login(token);
  }
}