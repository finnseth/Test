import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { NetworkControlComponent } from './networkcontrol.component';
import { NetworkControlService } from './networkcontrol.service';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ],

    declarations: [
        NetworkControlComponent
    ],

    providers: [
        NetworkControlService
    ]

})
export class NetworkControlModule {}
