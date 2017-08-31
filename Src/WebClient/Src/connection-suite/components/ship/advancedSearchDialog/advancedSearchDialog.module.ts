import { DataTableModule, InputTextModule, SharedModule, SplitButtonModule } from 'primeng/primeng';

import { AdvancedSearchDialogComponent } from './advancedSearchDialog.component';
import { CommonModule } from '@angular/common';
import { DialogModule } from 'primeng/primeng';
import { DropdownModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { ShipSelectorService } from '../shipSelector.service';

@NgModule({
    imports: [
        CommonModule,
        DialogModule,
        DataTableModule,
        SharedModule,
        DropdownModule,
        InputTextModule,
        SplitButtonModule
    ],

    exports: [
        AdvancedSearchDialogComponent
    ],

    declarations: [
        AdvancedSearchDialogComponent
    ],

    providers: [
        ShipSelectorService
    ]
})

export class AdvancedSearchDialogModule { }
