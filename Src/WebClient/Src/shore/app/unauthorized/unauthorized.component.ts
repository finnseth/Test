import { Component } from '@angular/core';

import { AuthenticationService } from '../../../common/services/authentication.service';

@Component({
    templateUrl: './unauthorized.component.html'
})
export class UnauthorizedComponent {
    constructor(private authService: AuthenticationService) {
        this.authService.startSigninMainWindow();
    }
}

