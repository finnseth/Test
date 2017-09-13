import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { DuaSpinnerComponent } from './dua-spinner.component';
import { NgModule } from '@angular/core';
import { SpinnerModule } from 'primeng/primeng';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        SpinnerModule
    ],

    declarations: [
        DuaSpinnerComponent
    ],

    exports: [
        DuaSpinnerComponent
    ]

})
export class DuaSpinnerModule {}
