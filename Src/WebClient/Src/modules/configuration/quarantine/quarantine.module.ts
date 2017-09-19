import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    ListboxModule,
    PanelModule
} from 'primeng/primeng';

import { ApplyRowModule } from 'dualog-common/components/applyRow/applyRow.module';
import { AquaCardModule } from 'dualog-common/components/aquaCard/aquaCard.module';
import { BlueCardModule } from 'dualog-common/components/blueCard/blueCard.module';
import { CommonModule } from '@angular/common';
import { CopyModule } from '../../../connection-suite-shore/components/copy/copy.module';
import { CurrentShipService } from '../../../connection-suite-shore/services/currentship.service';
import { DuaCheckboxModule } from 'dualog-common/components/dua-checkbox/dua-checkbox.module';
import { DuaCompareDirective } from 'dualog-common/directives/compare.directive';
import { DuaInputEmailModule } from 'dualog-common/components/dua-input-email/dua-input-email.module';
import { DuaSpinnerModule } from 'dualog-common/components/dua-spinner/dua-spinner.module';
import { FleetCardHeaderModule } from 'connection-suite-shore/components/fleetCardHeader/fleetCardHeader.module';
import { NgModule } from '@angular/core';
import { PendingChangesGuard } from '../../../connection-suite-shore/services/pending_changes.service';
import { QuarantineComponent } from './quarantine.component';
import { QuarantineService } from './quarantine.service';
import { ShipCardHeaderModule } from 'connection-suite-shore/components/shipCardHeader/shipCardHeader.module';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        ListboxModule,
        PanelModule,
        ApplyRowModule,
        ShipCardHeaderModule,
        FleetCardHeaderModule,
        AquaCardModule,
        BlueCardModule,
        DuaCheckboxModule,
        DuaInputEmailModule,
        DuaSpinnerModule,
        CopyModule
    ],

    declarations: [
        QuarantineComponent,
        DuaCompareDirective
    ],

    providers: [
        QuarantineService,
        PendingChangesGuard,
        CurrentShipService
    ]

})
export class QuarantineModule {}
