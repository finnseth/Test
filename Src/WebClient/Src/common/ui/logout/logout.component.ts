import { Component, OnInit } from '@angular/core';

import { AuthenticationService } from './../../services/authentication.service';

@Component({
  selector: 'logout-button',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})

export class LogoutComponent implements OnInit {

  constructor(private securityService: AuthenticationService) {

  }

  ngOnInit() {
  }

  logout() {

      this.securityService.startSignoutMainWindow();
  }

}
