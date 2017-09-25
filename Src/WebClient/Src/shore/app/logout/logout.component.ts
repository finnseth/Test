import { Component } from '@angular/core';

import { AuthenticationService } from '../../../common/services/authentication.service';

@Component({
    templateUrl: './logout.component.html'
})
export class LogoutComponent {
    constructor(private authService: AuthenticationService) {
        this.authService.startSignoutMainWindow();
    }
}

