import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { DButtonComponent } from './dButton.component';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ButtonModule,
    ],

    exports: [
        DButtonComponent
    ],

    declarations: [
        DButtonComponent
    ]
})

export class DButtonModule { }
