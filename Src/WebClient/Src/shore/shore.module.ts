import { CommonModule } from '@angular/common';
import { HttpModule } from '@angular/http';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';

import { InfrastructureModule } from '../infrastructure/infrastructure.module';

import { CommonsModule } from '../common/common.module';
import { ApiService } from './../common/services/api-base.service';

import { UserGroupService } from './services/userGroup.service';


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
        UserGroupService
    ]

})
export class ShoreModule { }
