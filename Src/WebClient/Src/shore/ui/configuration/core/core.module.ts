import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import {
    DataGridModule,
    PanelModule,
} from 'primeng/primeng';

import { CommunicationModule } from '../communication/communication.module';
import { CoreComponent } from './core.component';
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
        CompanyModule
    ],

    declarations: [
       CoreComponent
    ],

    providers: [
    ]
})
export class CoreModule {}
