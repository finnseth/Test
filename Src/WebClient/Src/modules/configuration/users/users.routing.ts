import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { EditUserComponent } from './edit-user/edit-user.component';
import { ListUsersComponent } from './list-users';
import { ModuleWithProviders } from '@angular/core';

export const userRoutes: Routes = [
    {
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
    }
];

export const userRouting: ModuleWithProviders = RouterModule.forChild(userRoutes);
