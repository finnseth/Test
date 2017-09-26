import { Component, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

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

    constructor( private userApiService: UserApiService, private userService: UserService, private router: Router ) {
    }

    public selectUser(user: User): void {
        this.router.navigateByUrl(`configuration/organization/users/${user.id}`)
    }

    loadUsers(event) {
        const result = this.userApiService.GetUsers( new PaginationInfo( event.first, event.rows, true ) ).share();
        this.users = result.map( ul => ul.value );
        this.totalCount = result.map( ul => ul.totalCount );
    }
}
