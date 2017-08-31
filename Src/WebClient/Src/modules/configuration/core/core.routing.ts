import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { CoreComponent } from './core.component';
import { EditUserComponent } from './../users/edit-user/edit-user.component';
import { ListUsersComponent } from './../users/list-users';
import { ModuleWithProviders } from '@angular/core';
import { communicationRoutes } from '../communication/communication.routing';
import { userRoutes } from './../users/users.routing';

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
               /*{
                    path: 'users',
                    component: ListUsersComponent,
                    data: {
                        permissions: PermissionMap.Config.Email.Quarantine, // @todo wrong permission
                        label: 'Users',
                        icon: 'fa-users'
                    },
                    canActivate: [ AuthGuard ]
                },
                {
                    path: 'users/:id',
                    component: EditUserComponent,
                    data: {
                        permissions: PermissionMap.Config.Email.Quarantine, // @todo wrong permission
                    },
                    canActivate: [ AuthGuard ]
                },*/
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
