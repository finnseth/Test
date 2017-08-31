import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { EmailComponent } from './email.component';
import { ModuleWithProviders } from '@angular/core';
import { QuarantineComponent } from './../quarantine/quarantine.component';

export const emailRoutes: Routes = [
    {
        path: 'email',
        data: {
            permissions: PermissionMap.Config.Email,
            label: 'E-mail'
        },
        children: [
            {
                path: 'technicalsetup',
                component: QuarantineComponent, // @todo wrong component
                data: {
                    permissions: PermissionMap.Config.Email.Technical,
                    label: 'Technical setup'
                },
                canActivate: [ AuthGuard ]
            },
            {
                path: 'quarantine',
                component: QuarantineComponent,
                data: {
                    permissions: PermissionMap.Config.Email.Quarantine,
                    label: 'Quarantine',
                    icon: 'dualog-quarantine-icon-16'
                },
                canActivate: [ AuthGuard ]
            },
            {
                path: 'distributionlist',
                component: QuarantineComponent, // @todo wrong component
                data: {
                    permissions: PermissionMap.Config.Email.Distributionlist,
                    label: 'Distribution list',
                    icon: 'dualog-distributionlist-icon-16'
                },
                canActivate: [ AuthGuard ]
            },
            {
                path : '',
                component: EmailComponent,
                pathMatch: 'full'
            }
        ],
        canActivate: [ AuthGuard ]
    }
];

export const emailRouting: ModuleWithProviders = RouterModule.forChild(emailRoutes);
