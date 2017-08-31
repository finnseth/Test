import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { HMenuBarComponent } from './hmenubar.component';
import { MenubarModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { OverlayPanelModule } from 'primeng/primeng';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MenubarModule,
        ButtonModule,
        OverlayPanelModule
    ],

    exports: [
        HMenuBarComponent
    ],

    declarations: [
        HMenuBarComponent,
    ]
})

export class HMenuBarModule { }
