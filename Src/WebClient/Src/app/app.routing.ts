import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { CanActivateChild } from '@angular/router/router';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { ModuleWithProviders } from '@angular/core';
import { UnauthorizedComponent } from './unauthorized/unauthorized.component';

export const routes: Routes = [
    {
        path: '',
        redirectTo: 'information',
        pathMatch: 'full'
    },
    {
        path: 'login',
        component: LoginComponent
    },
    {
        path: 'unauthorized',
        component: UnauthorizedComponent
    },
    {
        path: 'logout',
        component: LogoutComponent
    },
    {
        path: 'information',
        loadChildren: '../modules/information/information.module#InformationModule',
        data: {
            permissions: PermissionMap.Information,
            label: 'Information',
            icon: '/assets/topmenu/information.png'
        },
        canActivate: [ AuthGuard ]
    },
    {
        path: 'configuration',
        loadChildren: '../modules/configuration/configuration.module#ConfigurationModule',
        data: {
            permissions: PermissionMap.Config,
            label: 'Configuration',
            icon: '/assets/topmenu/config.png'
        },
        canActivate: [ AuthGuard ]
    },
   /* {
        path: 'operations',
        loadChildren: '../modules/configuration/configuration.module#ConfigurationModule', // @todo: wrong children
        data: {
            permissions: PermissionMap.Operations,
            label: 'Operations',
            icon: '/assets/topmenu/actions.png'
        },
        canActivate: [ AuthGuard ]
    },*/
    {
        path: 'Dualog',
        loadChildren: '../modules/dualog/dualog.module#DualogModule',
        data: {
            permissions: PermissionMap.Dualog.Overview,
            label: 'Dualog',
            icon: '/assets/topmenu/actions.png'
        },
        canActivate: [ AuthGuard ]
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);
