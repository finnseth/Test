import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import { CsInputComponent } from './input.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],

    declarations: [
        CsInputComponent
    ],

    exports: [
        CsInputComponent
    ]
})
export class CsInputModule { }

export * from './input.component';
