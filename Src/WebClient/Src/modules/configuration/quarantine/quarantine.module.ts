import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import {
    ListboxModule,
    PanelModule
} from 'primeng/primeng';

import { ApplyRowModule } from 'infrastructure/ui/applyRow/applyRow.module';
import { AquaCardModule } from 'infrastructure/ui/aquaCard/aquaCard.module';
import { BlueCardModule } from 'infrastructure/ui/blueCard/blueCard.module';
import { CommonModule } from '@angular/common';
import { CopyModule } from 'common/ui/copy/copy.module';
import { CurrentShipService } from 'shore/services/currentship.service';
import { DuaCheckboxModule } from 'infrastructure/ui/dua-checkbox/dua-checkbox.module';
import { DuaCompareDirective } from 'infrastructure/directives/compare.directive';
import { DuaInputEmailModule } from 'infrastructure/ui/dua-input-email/dua-input-email.module';
import { DuaSpinnerModule } from 'infrastructure/ui/dua-spinner/dua-spinner.module';
import { FleetCardHeaderModule } from 'common/ui/fleetCardHeader/fleetCardHeader.module';
import { NgModule } from '@angular/core';
import { PendingChangesGuard } from 'shore/services/pending_changes.service';
import { QuarantineComponent } from './quarantine.component';
import { QuarantineService } from './quarantine.service';
import { ShipCardHeaderModule } from 'common/ui/shipCardHeader/shipCardHeader.module';

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
