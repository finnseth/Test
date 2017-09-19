import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { FleetCardHeaderComponent } from './fleetCardHeader.component';
import { FleetCardSearchModule } from 'connection-suite-shore/components/fleetCardSearch/fleetCardSearch.module';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        FleetCardSearchModule
    ],

    exports: [
        FleetCardHeaderComponent
    ],

    declarations: [
        FleetCardHeaderComponent
    ]
})

export class FleetCardHeaderModule { }
