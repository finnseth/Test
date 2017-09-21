import { AccessRights, PermissionMap } from 'shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { AuthGuard } from './../../../services/auth-guard.service';
import { CoreComponent } from './core.component';
import { communicationRoutes } from '../communication/communication.routing';
import { userRoutes } from './../users/users.routing';
import { EditUserComponent } from './../users/edit-user/edit-user.component';
import { CompanyComponent } from './../company/company.component';

export const coreRoutes: Routes = [
    {
        path: 'core',
        data: {
            permissions: PermissionMap.Config.Email, // @todo: wrong permission
            label: 'Core',
            description: 'Core configuration of Dualog Connection Suite',
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
                        permissions: PermissionMap.Config.Core.Company,
                        label: 'Company'
                    },
                    canActivate: [ AuthGuard ]
                },
                ...userRoutes,
                ...communicationRoutes,
                {
                path : '',
                    component: CoreComponent,
                    pathMatch: 'full'
                }
        ],
        canActivate: [ AuthGuard ]
    }
];

export const coreRouting: ModuleWithProviders = RouterModule.forChild(coreRoutes);
