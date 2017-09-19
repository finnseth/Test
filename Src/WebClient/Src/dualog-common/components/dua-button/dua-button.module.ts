import { ButtonModule } from 'primeng/primeng';
import { CommonModule } from '@angular/common';
import { DuaButtonComponent } from './dua-button.component';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ButtonModule,
    ],

    exports: [
        DuaButtonComponent
    ],

    declarations: [
        DuaButtonComponent
    ]
})

export class DuaButtonModule { }
