import {
    ButtonModule,
    MegaMenuModule,
    MenubarModule,
    OverlayPanelModule,
} from 'primeng/primeng';
import { RouterModule, Routes } from '@angular/router';

import { CommonModule } from '@angular/common';
import { DualogComponent } from './dualog.component';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { dualogRouting } from './dualog.routing';

@NgModule({
    imports: [
        dualogRouting,
        ReactiveFormsModule,
        ButtonModule,
        OverlayPanelModule,
        RouterModule,
        MegaMenuModule,
        CommonModule,
        MenubarModule
    ],
    declarations: [
        DualogComponent
    ],
})
export class DualogModule {}
