import { RouterModule, Routes } from '@angular/router';
import { Component } from '@angular/core/core';
import { ModuleWithProviders } from '@angular/core';

import { PermissionMap } from './../../services/permission.service';
import { ConfigurationComponent } from './configuration.component';
import { organizationRoutes } from './organization/organization.routing';
import { emailRoutes } from './email/email.routing';
import { internetRoutes } from './internet/internet.routing';

export const configurationRoutes: Routes = [
    {
        path: '',
        component: ConfigurationComponent,
        data: {
            path: 'configuration'
        },
        children: [
            ...organizationRoutes,
            ...emailRoutes,
            ...internetRoutes
        ],
    },
];

export const configurationRouting: ModuleWithProviders = RouterModule.forChild(configurationRoutes);
