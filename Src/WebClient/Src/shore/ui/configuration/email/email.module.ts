import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';

import { EmailComponent } from './email.component';
import { QuarantineModule } from './../quarantine/quarantine.module';

@NgModule({
    imports: [
        CommonModule,
        QuarantineModule,
        DataGridModule,
        PanelModule,
        RouterModule,
    ],

    declarations: [
        EmailComponent
    ],

    providers: [
    ]
})
export class EmailModule {}
