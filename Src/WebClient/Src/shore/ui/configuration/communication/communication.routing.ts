
import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { AccessRights, PermissionMap } from './../../../services/permission.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { CommunicationComponent } from './communication.component';
import { MethodComponent } from './method/method.component';


export const communicationRoutes: Routes = [
    {
        path: 'communication',
        data: {
            permissions: PermissionMap.Config.Email, // todo: wrong permission
            label: 'Communication',
            icon: 'dualog-communication-icon-16',
            description: 'Manage communication systems',
            meta: [
                {tag: 'method', rank: 80 },
                {tag: 'carrier', rank: 80 },
                {tag: 'equipment', rank: 80 },
                {tag: 'IP', rank: 40 }
            ]
        },
        children: [
            {
                path: 'method',
                component: MethodComponent,
                data: {
                    permissions: PermissionMap.Config.Email.Quarantine, // @todo wrong permssion
                    label: 'Method',
                    description: 'Manage methods',
                    meta: [
                        {tag: 'carrier', rank: 90 },
                        {tag: 'equipment', rank: 90 },
                        {tag: 'IP', rank: 40 }
                    ]
                },
                canActivate: [AuthGuard]
            },
            {
                path: '',
                component: CommunicationComponent,
                pathMatch: 'full'
            }
        ],
        canActivate: [AuthGuard]
    }
];

export const communicationRouting: ModuleWithProviders = RouterModule.forChild(communicationRoutes);
