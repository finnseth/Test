import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { AccessRights, PermissionMap } from './../../../services/permission.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { ComputerRuleComponent } from '../networkcontrol/computerrule/computerrule.component';
import { InternetComponent } from './internet.component';
import { networkcontrolRoutes } from '../networkcontrol/networkcontrol.routing';

export const internetRoutes: Routes = [
    {
        path: 'internet',
        data: {
            permissions: PermissionMap.Config.Email, // todo: wrong permission
            label: 'Internet',
            description: 'Internet configuration'
        },
        children: [
            ...networkcontrolRoutes,
            {
                path: '',
                component: InternetComponent,
                pathMatch: 'full'
            }
        ],
        canActivate: [ AuthGuard ]
    }
];

export const emailRouting: ModuleWithProviders = RouterModule.forChild(internetRoutes);
