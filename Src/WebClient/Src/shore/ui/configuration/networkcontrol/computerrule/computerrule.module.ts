import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { DataTableModule, DropdownModule, SharedModule } from 'primeng/primeng';

import { ApplyRowModule } from './../../../../../infrastructure/ui/applyRow/applyRow.module';

import { ComputerRuleComponent } from './computerrule.component';
import { NetworkControlService } from '../networkcontrol.service';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DataTableModule,
        SharedModule,
        DropdownModule,
        ApplyRowModule
    ],

    declarations: [
        ComputerRuleComponent
    ],

    providers: [
        NetworkControlService
    ]

})
export class ComputerRuleModule {}
