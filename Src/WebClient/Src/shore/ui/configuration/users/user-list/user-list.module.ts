import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

import {
    DataListModule
} from 'primeng/primeng';

import { UserListComponent } from './user-list.component';
import { UserListItemModule } from '../user-list-item/user-list-item.module'


// import { userRouting } from './users-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReactiveFormsModule,
        UserListItemModule,
        DataListModule
    ],

    declarations: [
        UserListComponent
    ],

    exports: [
        UserListComponent
    ],

    providers: [
    ]
})
export class UserListModule { }
