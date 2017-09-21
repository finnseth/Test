import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { CommunicationComponent } from './communication.component';
import { CommunicationService } from './communication.service';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ],

    declarations: [
        CommunicationComponent
    ],

    providers: [
        CommunicationService
    ]

})
export class CommunicationModule {}
