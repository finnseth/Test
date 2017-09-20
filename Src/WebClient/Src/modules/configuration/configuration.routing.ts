import { RouterModule, Routes } from '@angular/router';

import { Component } from '@angular/core/core';
import { ConfigurationComponent } from './configuration.component';
import { ModuleWithProviders } from '@angular/core';
import { PermissionMap } from 'shore/services/permission.service';
import { coreRoutes } from './core/core.routing';
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
            ...coreRoutes,
            ...emailRoutes,
            ...internetRoutes
        ],
    },
];

export const configurationRouting: ModuleWithProviders = RouterModule.forChild(configurationRoutes);
