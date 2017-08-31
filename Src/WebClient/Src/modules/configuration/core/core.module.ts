import {
    DataGridModule,
    PanelModule,
} from 'primeng/primeng';

import { CommonModule } from '@angular/common';
import { CommunicationModule } from '../communication/communication.module';
import { CoreComponent } from './core.component';
import { MethodModule } from '../communication/method/method.module';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { UsersModule } from './../users/users.module';

@NgModule({
    imports: [
        CommonModule,
        UsersModule,
        DataGridModule,
        PanelModule,
        CommunicationModule,
        MethodModule
    ],

    declarations: [
       CoreComponent
    ],

    providers: [
    ]
})
export class CoreModule {}
