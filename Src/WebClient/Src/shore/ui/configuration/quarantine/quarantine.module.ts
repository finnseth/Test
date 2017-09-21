import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    ListboxModule,
    PanelModule
} from 'primeng/primeng';

import { ApplyRowModule } from './../../../../infrastructure/ui/applyRow/applyRow.module';
import { AquaCardModule } from './../../../../infrastructure/ui/aquaCard/aquaCard.module';
import { BlueCardModule } from './../../../../infrastructure/ui/blueCard/blueCard.module';
import { DuaCheckboxModule } from './../../../../infrastructure/ui/dua-checkbox/dua-checkbox.module';
import { DuaCompareDirective } from './../../../../infrastructure/directives/compare.directive';
import { DuaInputEmailModule } from './../../../../infrastructure/ui/dua-input-email/dua-input-email.module';
import { DuaSpinnerModule } from './../../../../infrastructure/ui/dua-spinner/dua-spinner.module';

import { CopyModule } from './../../../../common/ui/copy/copy.module';
import { FleetCardHeaderModule } from './../../../../common/ui/fleetCardHeader/fleetCardHeader.module';
import { ShipCardHeaderModule } from './../../../../common/ui/shipCardHeader/shipCardHeader.module';

import { CurrentShipService } from './../../../services/currentship.service';
import { PendingChangesGuard } from './../../..//services/pending_changes.service';
import { QuarantineComponent } from './quarantine.component';
import { QuarantineService } from './quarantine.service';


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
