import {NgModule, ModuleWithProviders} from '@angular/core';
import { CsInputModule } from './input';
import { CsCheckboxModule } from './checkbox';
import { CsSelectModule } from './select';
import { CsOptionModule } from './option';

@NgModule({
    imports: [
        CsInputModule,
        CsSelectModule,
        CsOptionModule,
        CsCheckboxModule
    ],

    exports: [
        CsInputModule,
        CsSelectModule,
        CsOptionModule,
        CsCheckboxModule
    ]

})
export class CsFormsModule {
}
