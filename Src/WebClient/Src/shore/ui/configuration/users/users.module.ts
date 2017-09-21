import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import {
    AutoCompleteModule,
    ButtonModule,
    CheckboxModule,
    ChipsModule,
    DataTableModule,
    DialogModule,
    DropdownModule,
    GrowlModule,
    InputTextModule,
    MultiSelectModule,
    PanelModule,
    RadioButtonModule,
    SelectButtonModule,
    SharedModule,
    SpinnerModule,
    TabViewModule,
    ToolbarModule,
    TooltipModule,
    TreeNode,
    TreeTableModule
} from 'primeng/primeng';


import { EditUserComponent } from './edit-user/edit-user.component';
import { UserApiService } from './user-api.service';
import { UserDetailsModule } from './user-details/user-details.module';
import { UserListModule } from './user-list/user-list.module';
import { UserService } from './user.service';
import { UsersComponent } from './users.component';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        CommonModule,
        DropdownModule,
        InputTextModule,
        SpinnerModule,
        PanelModule,
        SelectButtonModule,
        GrowlModule,
        ButtonModule,
        TreeTableModule,
        DataTableModule,
        DialogModule,
        TooltipModule,
        ChipsModule,
        AutoCompleteModule,
        MultiSelectModule,
        CheckboxModule,
        RadioButtonModule,
        ToolbarModule,

        // internal imports
        UserListModule,
        UserDetailsModule
    ],

    declarations: [
        UsersComponent,
        EditUserComponent
    ],

    providers: [
        UserApiService,
        UserService
    ]
})
export class UsersModule { }
