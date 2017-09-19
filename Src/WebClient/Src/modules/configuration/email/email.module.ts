import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';

import { CommonModule } from '@angular/common';
import { EmailComponent } from './email.component';
import { NgModule } from '@angular/core';
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
