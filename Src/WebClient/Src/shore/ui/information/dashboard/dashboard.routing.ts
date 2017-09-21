
import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { AccessRights, PermissionMap } from './../../../services/permission.service';
import { AuthGuard } from './../../../services/auth-guard.service';
import { DashboardComponent } from './dashboard.component';
import { DashboardEditWidgetComponent } from './dashboard-editwidget.component';

export const dashboardRoutes: Routes = [
    {
        path: 'dashboard',
        data: {
            permissions: PermissionMap.Config.Email.Quarantine, // @todo: wrong permissions
            label: 'Dashboard',
            icon: 'dualog-email-icon-16' // @todo: wrong icon
        },
        children: [
            {
                path: 'widget/:id',
                component: DashboardEditWidgetComponent,
                data: {
                    permissions: PermissionMap.Config.Email.Quarantine, // @todo: wrong permissions
                }
            },
            {
                path : '',
                component: DashboardComponent,
                pathMatch: 'full',
                data: {
                    permissions: PermissionMap.Config.Email.Quarantine, // @todo: wrong permissions
                    label: 'View',
                    icon: 'dualog-email-icon-16' // @todo: wrong icon
                }
            }
        ],
        canActivate: [ AuthGuard ]
    }
];

export const dashboardRouting: ModuleWithProviders = RouterModule.forChild(dashboardRoutes);
