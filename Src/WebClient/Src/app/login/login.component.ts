import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from 'connection-suite-shore/services/authentication.service';
import { Router } from '@angular/router';
import { SessionService } from 'connection-suite-shore/services/session.service';

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
