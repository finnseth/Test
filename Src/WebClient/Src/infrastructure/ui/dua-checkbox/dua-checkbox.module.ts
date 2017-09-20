import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CheckboxModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { DuaCheckboxComponent } from './dua-checkbox.component';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        CheckboxModule,
    ],

    declarations: [
        DuaCheckboxComponent
    ],

    exports: [
        DuaCheckboxComponent
    ]

})
export class DuaCheckboxModule {}
