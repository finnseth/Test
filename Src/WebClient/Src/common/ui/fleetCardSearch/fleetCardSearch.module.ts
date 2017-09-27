import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ButtonModule, AutoCompleteModule } from 'primeng/primeng';

import { FleetCardAdvancedSearchModule } from '../fleetCardAdvancedSearch/fleetCardAdvancedSearch.module';
import { FleetCardSearchComponent } from './fleetCardSearch.component';
import { ShipCardSearchService } from '../shipCardSearch/shipCardSearch.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        FleetCardAdvancedSearchModule,
        AutoCompleteModule
    ],

    exports: [
        FleetCardSearchComponent
    ],

    declarations: [
        FleetCardSearchComponent
    ],
    providers: [
        ShipCardSearchService
    ]
})

export class FleetCardSearchModule { }
