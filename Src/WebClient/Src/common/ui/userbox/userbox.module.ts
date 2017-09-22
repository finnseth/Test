import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { MenuModule } from './../../../infrastructure/ui/menus/menu.module';

import { UserboxComponent } from './userbox.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        MenuModule
    ],

    declarations: [
       UserboxComponent
    ],

    exports: [
        UserboxComponent
    ]

})
export class UserboxModule {}
