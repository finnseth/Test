import {NgModule, ModuleWithProviders} from '@angular/core';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';

import { CsOptionComponent } from './option.component';

@NgModule({
    imports: [
        CommonModule,
        FormsModule
    ],

    declarations: [
        CsOptionComponent
    ],

    exports: [
        CsOptionComponent
    ]
})
export class CsOptionModule {
}

export * from './option.component';
