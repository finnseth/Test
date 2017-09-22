import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';

import { MenuModule } from './../../../../infrastructure/ui/menus/menu.module';

import { EmailComponent } from './email.component';
import { QuarantineModule } from './../quarantine/quarantine.module';
import { RestrictionComponent } from './../restriction/restriction.component';

@NgModule({
    imports: [
        CommonModule,
        QuarantineModule,
        DataGridModule,
        PanelModule,
        RouterModule,
        MenuModule
    ],

    declarations: [
        EmailComponent,
        RestrictionComponent
    ],

    providers: [
    ]
})
export class EmailModule {}
