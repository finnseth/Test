import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { InfrastructureModule } from '../infrastructure/infrastructure.module';

import { CommonsModule } from '../common/common.module';
import { AuthenticationService } from './../common/services/authentication.service';
import { SessionService } from './../common/services/session.service';
import { ApiService } from './../common/services/api-base.service';

import { MenuService } from './services/menu.service';
import { PermissionService } from './services/permission.service';
import { UserGroupService } from './services/userGroup.service';
import { AuthGuard } from './services/auth-guard.service';


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
        PermissionService,
        AuthenticationService,
        MenuService,
        AuthGuard,
        SessionService,
        UserGroupService
    ]

})
export class ShoreModule { }
