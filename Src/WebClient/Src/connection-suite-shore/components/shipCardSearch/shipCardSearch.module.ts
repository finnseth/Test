import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ShipCardAdvancedSearchModule } from '../shipCardAdvancedSearch/shipCardAdvancedSearch.module';
import { ShipCardSearchComponent } from './shipCardSearch.component';
import { ShipCardSearchService } from './shipCardSearch.service';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        ShipCardAdvancedSearchModule
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
