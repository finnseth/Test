import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

/*
import { ApiService } from './services/api-base.service';
import { AuthGuard } from './services/auth-guard.service';
import { MenuService } from './services/menu.service';
import { PermissionService } from './services/permission.service';
import { UserGroupService } from './services/userGroup.service';
*/
import { InfrastructureModule } from '../infrastructure/infrastructure.module';

import { CommonsModule } from './../common/common.module';
import { AuthenticationService } from './../common/services/authentication.service';
import { SessionService } from './../common/services/session.service';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        HttpModule,
        InfrastructureModule,
        CommonsModule,
    ],

    declarations: [
    ],

    providers: [
        AuthenticationService,
        SessionService
        /*
        PermissionService,
        MenuService,
        AuthGuard,
        UserGroupService
        */
    ]

})
export class ShipModule { }
