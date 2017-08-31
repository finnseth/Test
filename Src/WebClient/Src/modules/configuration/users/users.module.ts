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
    TreeTableModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { CardsModule } from 'connection-suite/components/cards/cards.module';
import { CommonModule } from '@angular/common';
import { EditUserComponent } from './edit-user/edit-user.component';
import { HorizontalSubMenuModule } from 'connection-suite/components/hSubMenu/hSubMenu.module';
import { ListUsersComponent } from './list-users';
import { NgModule } from '@angular/core';
import { UserService } from './user.service';

// import { userRouting } from './users.routing';

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
        RadioButtonModule
    ],

    declarations: [
        ListUsersComponent,
        EditUserComponent
    ],

    providers: [
        UserService,
    ]
})
export class UsersModule { }
