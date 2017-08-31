import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CardHeaderModule } from 'connection-suite/components/cardHeader/cardHeader.module';
import { CardModule } from 'connection-suite/components/card/card.module';
import { CardsComponent } from './cards.component';
import { CardsService } from './cards.service';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        ButtonModule,
        CardModule,
        CardHeaderModule
    ],

    exports: [
        CardsComponent
    ],

    declarations: [
        CardsComponent
    ],
    providers: [
        CardsService
    ]
})

export class CardsModule { }
