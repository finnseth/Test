import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { FleetCardAdvancedSearchModule } from '../fleetCardAdvancedSearch/fleetCardAdvancedSearch.module';
import { FleetCardSearchComponent } from './fleetCardSearch.component';
import { NgModule } from '@angular/core';
import { ShipCardSearchService } from '../shipCardSearch/shipCardSearch.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        FleetCardAdvancedSearchModule
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
