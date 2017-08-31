import {
    ButtonModule,
    MegaMenuModule,
    MenubarModule,
    OverlayPanelModule,
} from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';

import { CardModule } from 'connection-suite/components/card/card.module';
import { CommonModule } from '@angular/common';
import { DashboardModule } from './dashboard/dashboard.module';
import { InformationComponent } from './information.component';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { informationRouting } from './information.routing';

@NgModule({
    imports: [
        informationRouting,
        DashboardModule,
        ReactiveFormsModule,
        ButtonModule,
        OverlayPanelModule,
        RouterModule,
        MegaMenuModule,
        CommonModule,
        MenubarModule,
        CardModule
    ],
    declarations: [
        InformationComponent
    ],
})
export class InformationModule {}
