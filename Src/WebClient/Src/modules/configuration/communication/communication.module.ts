import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { CommunicationComponent } from './communication.component';
import { CommunicationService } from './communication.service';
import { NgModule } from '@angular/core';

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
