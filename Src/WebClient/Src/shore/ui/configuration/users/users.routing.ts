import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { PermissionMap } from './../../../services/permission.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { EditUserComponent } from './edit-user/edit-user.component';
import { UsersComponent } from './users.component';

export const userRoutes: Routes = [
    {
        path: 'users',
        component: UsersComponent,
        data: {
            permissions: PermissionMap.Config.Organization.User,
            label: 'Users',
            icon: 'fa-users'
        },
        canActivate: [ AuthGuard ]
    },
    {
        path: 'users/:id',
        component: UsersComponent,
        data: {
            permissions: PermissionMap.Config.Organization.UserGroup,
        },
        canActivate: [ AuthGuard ]
    }
];

export const userRouting: ModuleWithProviders = RouterModule.forChild(userRoutes);
