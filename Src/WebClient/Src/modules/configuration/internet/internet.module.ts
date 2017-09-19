import {
    DataGridModule,
    PanelModule
} from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';

import { CommonModule } from '@angular/common';
import { ComputerRuleModule } from './../networkcontrol/computerrule/computerrule.module';
import { InternetComponent } from './internet.component';
import { NetworkControlModule } from './../networkcontrol/networkcontrol.module';
import { NgModule } from '@angular/core';

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
