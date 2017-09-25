import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { OnDestroy } from '@angular/core/core';
import { RouterLink } from '@angular/router/router';

import { Subscription } from 'rxjs/Rx';

import { MainMenuService } from 'infrastructure/services/mainmenu.service';

import { AuthenticationService } from './../../common/services/authentication.service';

import { mainMenu } from './app.mainmenu';
import { userMenu } from './app.usermenu';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {

  isAuthorizedObs: Observable<boolean>;
  isAuthorized = false;

  constructor(
    private authenticationService: AuthenticationService,
    private mainmenuService: MainMenuService) {
      this.isAuthorizedObs = authenticationService.isLoggedInObs();
      mainmenuService.items = mainMenu;
      mainmenuService.userMenu = userMenu;
  }

  public ngOnInit(): void {
    this.isAuthorizedObs.subscribe((isAuth) => {
      if (isAuth) {
        this.isAuthorized = isAuth;
      }
    });
  }

  public ngOnDestroy(): void {
  }
}
