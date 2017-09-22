import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { SearchboxComponent } from './searchbox.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ],

    declarations: [
        SearchboxComponent
    ],

    exports: [
        SearchboxComponent
    ]

})
export class SearchboxModule {}
