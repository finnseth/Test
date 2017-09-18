import { NgModule } from '@angular/core';
import {
    ButtonModule
} from 'primeng/primeng';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { UserListItemComponent } from './user-list-item.component';


// import { userRouting } from './users-routing.module';

@NgModule({
    imports: [
        FormsModule,
        ReactiveFormsModule,
        ButtonModule
    ],

    declarations: [
        UserListItemComponent
    ],

    exports: [
        UserListItemComponent
    ],

    providers: [
    ]
})
export class UserListItemModule { }
