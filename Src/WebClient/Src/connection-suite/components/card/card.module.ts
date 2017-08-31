import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule } from 'primeng/primeng';
import { CardComponent } from './Card.component';
import { CommonModule } from '@angular/common';
import { DButtonModule } from '../dButton/dButton.module';
import { MenubarModule } from 'primeng/primeng';
import { NgModule } from '@angular/core';

@NgModule({
    imports: [
        CommonModule,
        ReactiveFormsModule,
        FormsModule,
        MenubarModule,
        ButtonModule,
        DButtonModule
    ],

    exports: [
        CardComponent
    ],

    declarations: [
        CardComponent
    ]
})

export class CardModule { }
