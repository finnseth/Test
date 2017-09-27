import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { AutoCompleteModule } from 'primeng/primeng';

import { ShipCardAdvancedSearchModule } from '../shipCardAdvancedSearch/shipCardAdvancedSearch.module';
import { ShipCardSearchComponent } from './shipCardSearch.component';
import { ShipCardSearchService } from './shipCardSearch.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        ShipCardAdvancedSearchModule,
        AutoCompleteModule
    ],

    exports: [
        ShipCardSearchComponent
    ],

    declarations: [
        ShipCardSearchComponent
    ],
    providers: [
        ShipCardSearchService
    ]
})

export class ShipCardSearchModule { }
