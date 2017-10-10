import { PermissionMap } from 'shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { PendingChangesGuard } from './../../../services/pending_changes.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { OrganizationComponent } from './organization.component';
import { communicationRoutes } from '../communication/communication.routing';
import { userRoutes } from './../users/users.routing';
import { EditUserComponent } from './../users/edit-user/edit-user.component';
import { CompanyComponent } from './../company/company.component';

export const organizationRoutes: Routes = [
    {
        path: 'organization',
        data: {
            permissions: PermissionMap.Config.Email, // @todo: wrong permission
            label: 'Organization',
            description: 'Organization configuration of Dualog Connection Suite',
            meta: [
                {tag: 'users', rank: 60 },
                {tag: 'communication', rank: 60 },
                {tag: 'method', rank: 60 }
            ]
        },
        children: [
                {
                    path: 'company',
                    component: CompanyComponent,
                    data: {
                        permissions: PermissionMap.Config.Organization.Company,
                        label: 'Company'
                    },
                    canActivate: [ AuthGuard ],
                    canDeactivate : [ PendingChangesGuard ]
                },
                ...userRoutes,
                ...communicationRoutes,
                {
                path : '',
                    component: OrganizationComponent,
                    pathMatch: 'full'
                }
        ],
        canActivate: [ AuthGuard ]
    }
];

export const organizationRouting: ModuleWithProviders = RouterModule.forChild(organizationRoutes);
