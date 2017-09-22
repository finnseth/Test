import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { TopMenuItemComponent } from './topmenu-item.component';


@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
    ],

    declarations: [
        TopMenuItemComponent
    ],

    exports: [
        TopMenuItemComponent
    ]

})
export class TopMenuItemModule {}
