import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { NetworkControlComponent } from './networkcontrol.component';
import { NetworkControlService } from './networkcontrol.service';
import { NgModule } from '@angular/core';

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
