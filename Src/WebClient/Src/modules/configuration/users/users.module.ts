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
    TooltipModule,
    TreeNode,
    TreeTableModule,
    ToolbarModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CardsModule } from 'connection-suite/components/cards/cards.module';
import { CommonModule } from '@angular/common';
import { EditUserComponent } from './edit-user/edit-user.component';
import { HorizontalSubMenuModule } from 'connection-suite/components/hSubMenu/hSubMenu.module';
import { UsersComponent } from './users.component';
import { NgModule } from '@angular/core';
import { UserApiService } from './user-api.service';
import { UserService } from './user.service';

import { UserListModule } from './user-list/user-list.module';
import { UserDetailsModule } from './user-details/user-details.module';

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
        HorizontalSubMenuModule,
        CardsModule,
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
