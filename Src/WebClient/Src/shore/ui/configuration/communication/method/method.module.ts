import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { CommunicationService } from '../communication.service';
import { MethodComponent } from './method.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ],

    declarations: [
        MethodComponent
    ],

    providers: [
        CommunicationService
    ]

})
export class MethodModule {}
