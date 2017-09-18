import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Message, TreeNode, SelectItem } from 'primeng/primeng';
import { User, UserDetail, UserListDetails } from './user-api.service';
import { UserService } from './user.service';

@Component({
    templateUrl: './users.component.html',
    styleUrls: [ './users.component.scss' ]
})
export class UsersComponent {

    constructor( public userService: UserService ) {
    }
}
