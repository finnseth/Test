import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { OnDestroy } from '@angular/core/core';
import { RouterLink } from '@angular/router/router';
import { Subscription } from 'rxjs/Rx';

import { MainMenuService } from './../../infrastructure/services/mainmenu.service';

import { AuthenticationService } from './../../common/services/authentication.service';
import { SessionService } from './../../common/services/session.service';

import { MenuService } from '../services/menu.service';
import { AccessRights, PermissionService } from '../services/permission.service';
import { mainMenu } from './app.mainmenu';
import { userMenu } from './app.usermenu';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'Welcome';
  redirectInfo = 'You are getting redirected for log in';
  IsAuthorizedObs: Observable<boolean>;
  IsAuthorized = false;

  constructor(
    private authenticationService: AuthenticationService,
    private permissionService: PermissionService,
    private menuService: MenuService,
    private mainmenuService: MainMenuService,
    private router: Router,
    private sessionService: SessionService) {
    this.IsAuthorizedObs = authenticationService.isLoggedInObs();
    mainmenuService.setItems(mainMenu);
    mainmenuService.userMenu = userMenu;
  }

  public ngOnInit(): void {

    this.IsAuthorizedObs.subscribe((isAuth) => {
      if (isAuth) {
        // Make sure the permissions are in place
        const getpermissions = this.permissionService.getPermissions().subscribe((permissions) => {
          this.IsAuthorized = isAuth;
          // this.router.navigate([this.sessionService.GetReturnUrl()]);
          getpermissions.unsubscribe();
        });
      }
    });
  }

  public ngOnDestroy(): void {
  }
}
