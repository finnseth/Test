import { CheckboxModule, DialogModule, DropdownModule } from 'primeng/primeng';
import { DataTableModule, InputTextModule, SharedModule, SplitButtonModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ShipCardAdvancedSearchComponent } from './shipCardAdvancedSearch.component';
import { ShipCardSearchService } from 'connection-suite/components/shipCardSearch/shipCardSearch.service';

@NgModule({
    imports: [
        CommonModule,
        DialogModule,
        DataTableModule,
        SharedModule,
        CheckboxModule,
        DropdownModule,
        InputTextModule,
        SplitButtonModule,
        ReactiveFormsModule,
        FormsModule
    ],

    exports: [
        ShipCardAdvancedSearchComponent
    ],

    declarations: [
        ShipCardAdvancedSearchComponent
    ],

    providers: [
        ShipCardSearchService
    ]
})

export class ShipCardAdvancedSearchModule { }
