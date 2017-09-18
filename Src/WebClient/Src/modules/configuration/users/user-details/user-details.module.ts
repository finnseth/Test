import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserDetailsComponent } from './user-details.component';
import { CommonModule } from '@angular/common';
import { ButtonModule, RadioButtonModule, AccordionModule, ToolbarModule, MessagesModule } from 'primeng/primeng';
import { UserPermissionListModule } from '../user-permission-list/user-permission-list.module';
import { UserUserGroupModule } from '../user-usergroup-list/user-usergroup-list.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        ButtonModule,
        AccordionModule,
        ToolbarModule,
        MessagesModule,

        UserPermissionListModule,
        UserUserGroupModule
    ],

    declarations: [
        UserDetailsComponent
    ],

    exports: [
        UserDetailsComponent
    ],

    providers: [
    ]
})
export class UserDetailsModule { }
