import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';

import { ComputerRuleModule } from './../networkcontrol/computerrule/computerrule.module';
import { InternetComponent } from './internet.component';
import { NetworkControlModule } from './../networkcontrol/networkcontrol.module';

@NgModule({
    imports: [
        CommonModule,
        NetworkControlModule,
        DataGridModule,
        PanelModule,
        RouterModule,
        ComputerRuleModule,
    ],

    declarations: [
        InternetComponent
    ],

    providers: [
    ]
})
export class InternetModule {}
