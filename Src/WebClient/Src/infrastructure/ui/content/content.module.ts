import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { ContentComponent } from './content.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        RouterModule
    ],

    declarations: [
        ContentComponent
    ],

    exports: [
        ContentComponent
    ]

})
export class ContentModule {}
