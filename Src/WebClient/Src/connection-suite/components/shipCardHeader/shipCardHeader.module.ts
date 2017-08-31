import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ShipCardHeaderComponent } from './ShipCardHeader.component';
import { ShipCardSearchModule } from 'connection-suite/components/shipCardSearch/shipCardSearch.module';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        ShipCardSearchModule
    ],

    exports: [
        ShipCardHeaderComponent
    ],

    declarations: [
        ShipCardHeaderComponent
    ]
})

export class ShipCardHeaderModule { }
