import { AccessRights, PermissionMap } from 'connection-suite-shore/services/permission.service';
import { RouterModule, Routes } from '@angular/router';

import { AuthGuard } from 'connection-suite-shore/services/auth-guard.service';
import { ComputerRuleComponent } from './computerrule/computerrule.component';
import { ModuleWithProviders } from '@angular/core';
import { NetworkControlComponent } from './networkcontrol.component';

export const networkcontrolRoutes: Routes = [
    {
        path: 'networkcontrol',
        data: {
            permissions: PermissionMap.Config.Email, // todo: wrong permission
            label: 'Network Control',
            icon: 'dualog-networkcontrol-icon-16',
            description: 'Manage your Internet',
            meta: [
                {tag: 'networkcontrol', rank: 100 },
                {tag: 'internet', rank: 50 }
            ]
        },
        children: [
            {
                path: 'computerrules',
                component: ComputerRuleComponent,
                data: {
                    permissions: PermissionMap.Config.Email.Quarantine, // @todo wrong permission
                    label: 'Computer rules',
                    description: 'Computer rules in Network Control',
                    meta: [
                        {tag: 'computerrules', rank: 100 },
                        {tag: 'networkcontrol', rank: 80 },
                        {tag: 'network control', rank: 80 },
                        {tag: 'internet', rank: 40 }
                    ]
                },
                canActivate: [AuthGuard]
            },
            {
                path: '',
                component: NetworkControlComponent,
                pathMatch: 'full'
            }
        ],
        canActivate: [AuthGuard]
    }
];

export const networkcontrolRouting: ModuleWithProviders = RouterModule.forChild(networkcontrolRoutes);
