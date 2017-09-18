import { NgModule } from '@angular/core';
import {
    DataListModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserListComponent } from './user-list.component';
import { UserListItemModule } from '../user-list-item/user-list-item.module'
import { CommonModule } from '@angular/common';

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
