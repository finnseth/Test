import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { BlueCardComponent } from './blueCard.component';
import { CommonModule } from '@angular/common';
import { CopyModule } from 'common/ui/copy/copy.module';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        CopyModule
    ],

    exports: [
        BlueCardComponent
    ],

    declarations: [
        BlueCardComponent
    ]
})

export class BlueCardModule { }
