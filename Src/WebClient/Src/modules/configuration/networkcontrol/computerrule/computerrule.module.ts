import { DataTableModule, DropdownModule, SharedModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ApplyRowModule } from 'dualog-common/components/applyRow/applyRow.module';
import { CommonModule } from '@angular/common';
import { ComputerRuleComponent } from './computerrule.component';
import { NetworkControlService } from '../networkcontrol.service';
import { NgModule } from '@angular/core';

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
