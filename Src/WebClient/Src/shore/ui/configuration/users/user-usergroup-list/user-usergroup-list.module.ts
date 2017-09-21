import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { ButtonModule, RadioButtonModule, DataListModule, ToolbarModule, PickListModule } from 'primeng/primeng';

import { UserUserGroupComponent } from './user-usergroup-list.component';


@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        DataListModule,
        ToolbarModule,
        ButtonModule,
        PickListModule
    ],

    declarations: [
        UserUserGroupComponent
    ],

    exports: [
        UserUserGroupComponent
    ]


})
export class UserUserGroupModule {
}
