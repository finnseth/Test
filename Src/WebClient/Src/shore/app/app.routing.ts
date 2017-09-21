import { RouterModule, Routes } from '@angular/router';
import { CanActivateChild } from '@angular/router/router';
import { ModuleWithProviders } from '@angular/core';

import { AccessRights, PermissionMap } from '../../shore/services/permission.service';
import { AuthGuard } from '../../shore/services/auth-guard.service';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { PendingChangesGuard } from '../../shore/services/pending_changes.service';
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
        loadChildren: './../ui/information/information.module#InformationModule',
        data: {
            permissions: PermissionMap.Information,
            label: 'Information',
            icon: '/assets/topmenu/information.png'
        },
        canActivate: [ AuthGuard ]
    },
    {
        path: 'configuration',
        loadChildren: './../ui/configuration/configuration.module#ConfigurationModule',
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
        loadChildren: './../ui/dualog/dualog.module#DualogModule',
        data: {
            permissions: PermissionMap.Dualog.Overview,
            label: 'Dualog',
            icon: '/assets/topmenu/actions.png'
        },
        canActivate: [ AuthGuard ]
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);
