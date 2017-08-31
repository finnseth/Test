import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';

import { CardModule } from 'connection-suite/components/card/card.module';
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
        CardModule,
    ],

    declarations: [
        EmailComponent
    ],

    providers: [
    ]
})
export class EmailModule {}
