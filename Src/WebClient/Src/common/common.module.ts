import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';

import { OverlayPanelModule } from 'primeng/primeng';

import { InfrastructureModule } from '../infrastructure/infrastructure.module';
import { DuaStatusBarModule } from '../infrastructure/ui/dua-status-bar/dua-status-bar.module';
import { DuaVerticalBarModule } from '../infrastructure/ui/dua-vertical-bar/dua-vertical-bar.module';

import { GlogoComponent } from './ui/glogo/glogo.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        RouterModule,
        OverlayPanelModule,
        InfrastructureModule,
        DuaStatusBarModule,
        DuaVerticalBarModule,
    ],

    declarations: [
        GlogoComponent
    ],

    exports: []
})
export class CommonsModule {}
