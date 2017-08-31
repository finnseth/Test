import { ApplyRowComponent } from './applyRow.component';
import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { DButtonModule } from 'connection-suite/components/dButton/dButton.module';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ButtonModule,
        DButtonModule
    ],

    exports: [
        ApplyRowComponent
    ],

    declarations: [
        ApplyRowComponent
    ]
})

export class ApplyRowModule { }
