import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AquaCardComponent } from './aquacard.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
    ],

    exports: [
        AquaCardComponent
    ],

    declarations: [
        AquaCardComponent
    ]
})

export class AquaCardModule { }
