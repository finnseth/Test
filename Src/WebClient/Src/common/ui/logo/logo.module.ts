import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { LogoComponent } from './logo.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule
    ],

    declarations: [
        LogoComponent
    ],

    exports: [
        LogoComponent
    ]

})
export class LogoModule {}
