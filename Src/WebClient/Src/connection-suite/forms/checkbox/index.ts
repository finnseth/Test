import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import { CsCheckboxComponent } from './checkbox.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],

    declarations: [
        CsCheckboxComponent,
    ],

    exports: [
        CsCheckboxComponent,
    ]
})
export class CsCheckboxModule {
}

export * from './checkbox.component';
