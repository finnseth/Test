import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BlueCardComponent } from './bluecard.component';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule
    ],

    exports: [
        BlueCardComponent
    ],

    declarations: [
        BlueCardComponent
    ]
})

export class BlueCardModule { }
