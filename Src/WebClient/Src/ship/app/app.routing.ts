import { RouterModule, Routes } from '@angular/router';
import { CanActivateChild } from '@angular/router/router';
import { ModuleWithProviders } from '@angular/core';

import { AppComponent } from './app.component';

export const routes: Routes = [
    {
        path: '',
        component: AppComponent
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(routes);
