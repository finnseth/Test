import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { DuaInputComponent } from './dua-input.component';
import { InputTextModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        InputTextModule
    ],

    declarations: [
        DuaInputComponent
    ],

    exports: [
        DuaInputComponent
    ]

})
export class DuaInputModule {}
