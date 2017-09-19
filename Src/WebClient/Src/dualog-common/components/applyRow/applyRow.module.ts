import { ApplyRowComponent } from './applyRow.component';
import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { DuaButtonModule } from '../dua-button/dua-button.module';
import { NgModule } from '@angular/core';

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
