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

import { DualogComponent } from './dualog.component';
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
