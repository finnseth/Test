import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

// Infrastructure
import { ApplyRowModule } from './../../../../infrastructure/ui/applyRow/applyRow.module';
import { BlueCardModule } from './../../../../infrastructure/ui/blueCard/blueCard.module';
import { DuaInputEmailModule } from './../../../../infrastructure/ui/dua-input-email/dua-input-email.module';
import { DuaInputModule } from './../../../../infrastructure/ui/dua-input/dua-input.module';
import { DuaLabelModule } from './../../../../infrastructure/ui/dua-label/dua-label.module';

// Common
import { FleetCardHeaderModule } from './../../../../common/ui/fleetCardHeader/fleetCardHeader.module';

// Shore services
import { CurrentShipService } from './../../../services/currentship.service';
import { PendingChangesGuard } from './../../../services/pending_changes.service';

import { ShoreModule } from './../../../shore.module';

// Company
import { CompanyComponent } from './company.component';
import { CompanyService } from './../../../services/company.service';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        ApplyRowModule,
        FleetCardHeaderModule,
        BlueCardModule,
        DuaInputEmailModule,
        DuaInputModule,
        DuaLabelModule,
        ShoreModule
    ],

    declarations: [
        CompanyComponent
    ],

    providers: [
        CompanyService,
        PendingChangesGuard,
        CurrentShipService
    ]

})
export class CompanyModule {}
