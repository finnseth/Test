import { Route, Routes } from '@angular/router';
import { Injectable } from '@angular/core';
import { MenuItem } from 'primeng/primeng';
import { Observable } from 'rxjs/Rx';

import { AccessRights, Availability, Permission, PermissionService } from './permission.service';
import { ArrayHelpers } from '../../infrastructure/array';
import { SessionService } from './session.service';

@Injectable()
export class MenuService {

    constructor(
        private sessionService: SessionService,
        private permissionService: PermissionService ) {
    }

    /**
     * Check if the user got access to the menu item
     *
     * @param {any} _permissions
     * @returns {Observable<boolean>}
     *
     * @memberof AuthenticationService
     */
    public GetMenuAccess(_permissions): Observable<boolean> {

        const permissionsToCheck: Permission[] = this.permissionMapToArray(_permissions);
        // const userPermissions: Permission[] = this.permissionService.getPermissions();

        return this.permissionService.getPermissions().map( userPermissions => {

            for (const permissionToCheck of permissionsToCheck) {
                const userPermissionFound = userPermissions.find(
                    p => p.name.toLocaleLowerCase() === permissionToCheck.name.toLocaleLowerCase()
                );

                if (userPermissionFound === undefined) {
                    continue;
                }

                if (userPermissionFound.allowType >= permissionToCheck.allowType) {
                    return this.CheckAvailability(permissionToCheck.availability);
                }
            }

            return false;
        })
    }

    private permissionMapToArray(object: Object): any[] {
        let output: any[] = [];
        for (const key in object) {
            if (object.hasOwnProperty('allowType')) {
                output.push(object);
                break;
            } else if (object !== {}) {
                output = output.concat(this.permissionMapToArray(object[key]));
            }
        }
        return output;
    }

    /**
     * Checking if the availability is acceptable for the logged in user
     *
     * @private
     * @param {Availability} availability
     * @returns {boolean}
     *
     * @memberof AuthenticationService
     */
    public CheckAvailability(availability?: Availability): boolean {

        // Check if accessable for everyone
        if (availability === Availability.All) {
            return true;
        }

        // Check that accessable for shore users
        if (availability === Availability.onlyShore) {
            return true;
        }

        // Check if it is a Dualog feature
        if (availability === Availability.onlyDualog) {
            return this.sessionService.IsDualogAdmin;
        }

        return false;
    }
    /**
     * Build up an array of menu items based on a route
    */
    public BuildMenu(routes: Routes, parentPath: string = null): MenuItem[] {

        const menuItems: MenuItem[] = [];

        // Loop throug each configured routed to build up the menu
        for (const route of routes) {
            if (route.data !== undefined) {
                if (route.data.permissions !== undefined) {

                    // Check if any label is defined, if so this is a menu point
                    if (route.data.label !== undefined) {
                        if (this.GetMenuAccess(route.data.permissions)) {
                            menuItems.push(this.CreateMenuItem(route, parentPath));
                        }
                    }
                }
            } else if (route.children !== undefined) {
                return this.BuildMenu(route.children);
            }
        }

        // Sort the menu in alphabetical order
        menuItems.sort(ArrayHelpers.AscendingString('label'));
        return menuItems;
    }

    private CreateMenuItem(route: Route, parentPath?: string) {

         // If any parent path add it to the path
        let path = route.path;
        if (parentPath !== null ) {
            path = parentPath + '/' + path;
        }

        const menuItem = {
            label: route.data.label,
            icon: route.data.icon,
            routerLink: path,
            items: undefined
        };

        if (route.children !== undefined) {
            menuItem.items = this.BuildMenu(route.children, path);
        }
        return menuItem;
    }
}
