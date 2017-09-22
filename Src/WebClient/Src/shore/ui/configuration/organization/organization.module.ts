import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import {
    DataGridModule,
    PanelModule,
} from 'primeng/primeng';

import { MenuModule } from './../../../../infrastructure/ui/menus/menu.module';

import { CommunicationModule } from '../communication/communication.module';
import { OrganizationComponent } from './organization.component';
import { MethodModule } from '../communication/method/method.module';
import { UsersModule } from './../users/users.module';
import { CompanyModule } from './../company/company.module';

@NgModule({
    imports: [
        CommonModule,
        UsersModule,
        DataGridModule,
        PanelModule,
        CommunicationModule,
        MethodModule,
        CompanyModule,
        MenuModule
    ],

    declarations: [
        OrganizationComponent
    ]
})
export class OrganizationModule {}
