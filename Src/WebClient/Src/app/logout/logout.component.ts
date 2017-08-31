import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Component } from '@angular/core';

@Component({
    templateUrl: './logout.component.html'
})
export class LogoutComponent {
    constructor(private authService: AuthenticationService) {
        this.authService.startSignoutMainWindow();
    }
}

