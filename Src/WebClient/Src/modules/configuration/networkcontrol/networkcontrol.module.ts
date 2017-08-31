import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CardModule } from 'connection-suite/components/card/card.module';
import { CardsModule } from 'connection-suite/components/cards/cards.module';
import { CommonModule } from '@angular/common';
import { NetworkControlComponent } from './networkcontrol.component';
import { NetworkControlService } from './networkcontrol.service';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        CardModule,
        CardsModule
    ],

    declarations: [
        NetworkControlComponent
    ],

    providers: [
        NetworkControlService
    ]

})
export class NetworkControlModule {}
