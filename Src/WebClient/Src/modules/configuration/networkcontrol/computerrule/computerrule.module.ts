import { DataTableModule, DropdownModule, SharedModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ApplyRowModule } from 'connection-suite/components/applyRow/applyRow.module';
import { CardHeaderModule } from 'connection-suite/components/cardHeader/cardHeader.module';
import { CardsModule } from 'connection-suite/components/cards/cards.module';
import { CommonModule } from '@angular/common';
import { ComputerRuleComponent } from './computerrule.component';
import { NetworkControlService } from '../networkcontrol.service';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        CardsModule,
        CardHeaderModule,
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
