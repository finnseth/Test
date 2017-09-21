import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { AuthenticationService } from './../../../services/authentication.service';

// import { MenuItem } from 'primeng/primeng';

@Component({
  templateUrl: './internet.component.html',
  styleUrls: [ './internet.component.scss' ]
})
export class InternetComponent implements OnInit {

  // private menuItems: MenuItem[] = [];

  constructor(private authService: AuthenticationService, private route: ActivatedRoute) {
  }

  public ngOnInit(): void {

    // this.menuItems = this.authService.BuildMenu(this.route.parent.routeConfig.children);
  }
}
