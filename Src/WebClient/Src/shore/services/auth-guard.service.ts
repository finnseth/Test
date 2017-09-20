import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { CanActivateChild } from '@angular/router/router';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Rx';

import { AuthenticationService } from './authentication.service';
import { MenuService } from './menu.service';
import { PermissionService } from './permission.service';

@Injectable()
export class AuthGuard implements CanActivate {

    constructor(
        private authService: AuthenticationService,
        private menuService: MenuService,
        private router: Router ) {
    }

    public canActivate (route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {

        this.authService.isLoggedInObs().subscribe( loggedIn => {
            if (!loggedIn) {
                this.router.navigate(['unauthorized']);
            }
        });

        if (this.authService.GetLoggedIn()) {

            return this.menuService.GetMenuAccess(route.data['permissions']);

            // if (this.permissionService.getPermissions() === null) {
            //     this.permissionService.retrievePermissions().subscribe((permissions) => {
            //         return this.menuService.GetMenuAccess(route.data['permissions']);
            //     });
            // } else {
            //     return Observable.of(this.menuService.GetMenuAccess(route.data['permissions']));
            // }
        } else {
            return Observable.of(false);
        }
    }
}
