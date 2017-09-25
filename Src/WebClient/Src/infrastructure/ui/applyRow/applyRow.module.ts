import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ButtonModule } from 'primeng/primeng';

import { ApplyRowComponent } from './applyRow.component';
import { DuaButtonModule } from '../dua-button/dua-button.module';

@NgModule({
    imports: [
        CommonModule,
        ButtonModule,
        DuaButtonModule
    ],

    exports: [
        ApplyRowComponent
    ],

    declarations: [
        ApplyRowComponent
    ]
})

export class ApplyRowModule { }
