import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserUserGroupComponent } from './user-usergroup-list.component';
import { ButtonModule, RadioButtonModule, DataListModule, ToolbarModule, PickListModule } from 'primeng/primeng';

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
