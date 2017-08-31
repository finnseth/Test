import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import { CsSelectComponent } from './select.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],

    declarations: [
        CsSelectComponent,
    ],

    exports: [
        CsSelectComponent,
    ]
})
export class CsSelectModule {
}

export * from './select.component';
