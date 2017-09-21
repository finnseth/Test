import { RouterModule, Routes } from '@angular/router';
import { ModuleWithProviders } from '@angular/core';

import { DualogComponent } from './dualog.component';

export const dualogRoutes: Routes = [ {
        path: '',
        component: DualogComponent,
        children: [
        ]
    },
];

export const dualogRouting: ModuleWithProviders = RouterModule.forChild(dualogRoutes);

