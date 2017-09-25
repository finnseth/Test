import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { Observable } from 'rxjs/Rx';

import { PaginationInfo } from './../../../../../common/services/api-base.service';

import { User, UserApiService, UserListDetails } from '../user-api.service';
import { UserService } from '../user.service';

@Component({
    selector: 'dualog-user-list',
    templateUrl: './user-list.component.html',
    styleUrls: [ './user-list.component.css' ]
})

export class UserListComponent {

    users: Observable<User[]>;
    totalCount: Observable<number>;

    constructor( private userApiService: UserApiService, private userService: UserService ) {
    }

    public selectUser(user: User): void {
        this.userService.currentUser = user;
    }

    loadUsers(event) {
        const result = this.userApiService.GetUsers( new PaginationInfo( event.first, event.rows, true ) ).share();
        this.users = result.map( ul => ul.value );
        this.totalCount = result.map( ul => ul.totalCount );
    }
}
