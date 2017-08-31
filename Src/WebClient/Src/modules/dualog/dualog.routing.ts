import { RouterModule, Routes } from '@angular/router';

import { DualogComponent } from './dualog.component';
import { ModuleWithProviders } from '@angular/core';

export const dualogRoutes: Routes = [ {
        path: '',
        component: DualogComponent,
        children: [
        ]
    },
];

export const dualogRouting: ModuleWithProviders = RouterModule.forChild(dualogRoutes);

