import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { DashboardComponent } from './dashboard/dashboard.component';
import { InformationComponent } from './information.component';
import { dashboardRoutes } from './dashboard/dashboard.routing';

export const informationRoutes: Routes = [ {
        path: '',
        component: InformationComponent,
        data: {
            path: 'information'
        },
        children: [
            ...dashboardRoutes
        ]
    },
];

export const informationRouting: ModuleWithProviders = RouterModule.forChild(informationRoutes);

