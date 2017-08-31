import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CardHeaderComponent } from './cardHeader.component';
import { CommonModule } from '@angular/common';
import { MenubarModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';
import { ShipSelectorModule } from 'connection-suite/components/ship/shipSelector.module';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MenubarModule,
        ButtonModule,
        ShipSelectorModule
    ],

    exports: [
        CardHeaderComponent
    ],

    declarations: [
        CardHeaderComponent
    ]
})

export class CardHeaderModule { }
