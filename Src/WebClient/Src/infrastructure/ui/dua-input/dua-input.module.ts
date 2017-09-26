import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { InputTextModule } from 'primeng/primeng';

import { DuaInputComponent } from './dua-input.component';
import { DuaRequiredDirective } from './../../directives/required.directive';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        InputTextModule
    ],

    declarations: [
        DuaInputComponent,
        DuaRequiredDirective
    ],

    exports: [
        DuaInputComponent
    ]

})
export class DuaInputModule {}
