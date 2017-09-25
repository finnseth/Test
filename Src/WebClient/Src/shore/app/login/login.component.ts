import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthenticationService } from '../../../common/services/authentication.service';
import { SessionService } from '../../../common/services/session.service';

@Component({
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

    constructor( private authService: AuthenticationService, private sessionService: SessionService, private router: Router ) {
        this.authService.endSigninMainWindow();
    }

    ngOnInit() {
        // this.router.navigate([this.sessionService.GetReturnUrl()]);
    }
}
