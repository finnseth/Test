import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { AdvancedSearchDialogModule } from './advancedSearchDialog/advancedSearchDialog.module';
import { AutoCompleteModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ShipSelectorComponent } from './shipSelector.component';
import { ShipSelectorService } from './shipSelector.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        AutoCompleteModule,
        AdvancedSearchDialogModule
    ],

    exports: [
        ShipSelectorComponent
    ],

    declarations: [
        ShipSelectorComponent
    ],

    providers: [ ShipSelectorService ]
})

export class ShipSelectorModule { }
