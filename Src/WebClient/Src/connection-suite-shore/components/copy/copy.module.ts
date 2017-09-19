import { ButtonModule, DataTableModule, DialogModule, SharedModule } from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { CopyComponent } from './copy.component';
import { DuaButtonModule } from 'dualog-common/components/dua-button/dua-button.module';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        DialogModule,
        DataTableModule,
        SharedModule,
        ButtonModule,
        DuaButtonModule
    ],

    exports: [
        CopyComponent
    ],

    declarations: [
        CopyComponent
    ]
})

export class CopyModule { }
