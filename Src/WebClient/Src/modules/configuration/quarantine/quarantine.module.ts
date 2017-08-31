import {
    CheckboxModule,
    InputTextModule,
    ListboxModule,
    PanelModule,
    SpinnerModule,
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ApplyRowModule } from 'connection-suite/components/applyRow/applyRow.module';
import { AquaCardModule } from 'connection-suite/components/aquaCard/aquaCard.module';
import { BlueCardModule } from 'connection-suite/components/blueCard/blueCard.module';
import { CardsModule } from 'connection-suite/components/cards/cards.module';
import { CommonModule } from '@angular/common';
import { FleetCardHeaderModule } from 'connection-suite/components/fleetCardHeader/fleetCardHeader.module';
import { NgModule } from '@angular/core';
import { QuarantineComponent } from './quarantine.component';
import { QuarantineService } from './quarantine.service';
import { ShipCardHeaderModule } from 'connection-suite/components/shipCardHeader/shipCardHeader.module';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        ListboxModule,
        InputTextModule,
        CheckboxModule,
        SpinnerModule,
        PanelModule,
        CardsModule,
        ApplyRowModule,
        ShipCardHeaderModule,
        FleetCardHeaderModule,
        AquaCardModule,
        BlueCardModule
    ],

    declarations: [
        QuarantineComponent
    ],

    providers: [
        QuarantineService
    ]

})
export class QuarantineModule {}
