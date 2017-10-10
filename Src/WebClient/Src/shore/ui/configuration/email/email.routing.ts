import { CanDeactivate, RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { RestrictionComponent } from './../restriction/restriction.component';
import { PermissionMap } from './../../../services/permission.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { EmailComponent } from './email.component';
import { PendingChangesGuard } from './../../../services/pending_changes.service';
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
                    permissions: PermissionMap.Config.Email.TechnicalSetup,
                    label: 'Technical setup'
                },
                canActivate: [ AuthGuard ]
            },
            {
                path: 'restriction',
                data: {
                    permissions: PermissionMap.Config.Email.Restriction,
                    label: 'Restriction'
                },
                canActivate: [ AuthGuard ],
                children: [
                    {
                        path: 'quarantine',
                        component: QuarantineComponent,
                        data: {
                            permissions: PermissionMap.Config.Email.Restriction,
                            label: 'Quarantine',
                            icon: 'dualog-quarantine-icon-16'
                        },
                        canActivate: [ AuthGuard ],
                        canDeactivate : [ PendingChangesGuard ]
                    },
                    {
                        path : '',
                        component: RestrictionComponent,
                        pathMatch: 'full'
                    }
                ]
            },
            {
                path: 'distributionlist',
                component: QuarantineComponent, // @todo wrong component
                data: {
                    permissions: PermissionMap.Config.Email.Address,
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
