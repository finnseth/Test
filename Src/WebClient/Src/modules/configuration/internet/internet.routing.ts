import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { ComputerRuleComponent } from '../networkcontrol/computerrule/computerrule.component';
import { InternetComponent } from './internet.component';
import { ModuleWithProviders } from '@angular/core';
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
