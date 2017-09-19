import { CheckboxModule, DialogModule, DropdownModule } from 'primeng/primeng';
import { DataTableModule, InputTextModule, SharedModule, SplitButtonModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { FleetCardAdvancedSearchComponent } from './fleetCardAdvancedSearch.component';
import { NgModule } from '@angular/core';
import { ShipCardSearchService } from 'connection-suite-shore/components/shipCardSearch/shipCardSearch.service';

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
        FleetCardAdvancedSearchComponent
    ],

    declarations: [
        FleetCardAdvancedSearchComponent
    ],

    providers: [
        ShipCardSearchService
    ]
})

export class FleetCardAdvancedSearchModule { }
