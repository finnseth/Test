import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CommonModule } from '@angular/common';
import { DuaInputEmailComponent } from './dua-input-email.component';
import { InputTextModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        InputTextModule
    ],

    declarations: [
        DuaInputEmailComponent
    ],

    exports: [
        DuaInputEmailComponent
    ]

})
export class DuaInputEmailModule {}
