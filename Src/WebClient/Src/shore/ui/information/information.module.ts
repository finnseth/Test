import { RouterModule, Routes } from '@angular/router';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';

import {
    ButtonModule,
    MegaMenuModule,
    MenubarModule,
    OverlayPanelModule,
} from 'primeng/primeng';

import { DashboardModule } from './dashboard/dashboard.module';
import { InformationComponent } from './information.component';
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
    ],
    declarations: [
        InformationComponent
    ],
})
export class InformationModule {}
