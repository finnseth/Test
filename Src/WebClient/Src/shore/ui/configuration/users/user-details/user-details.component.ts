import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

import { Observable } from 'rxjs/Rx';

import { Message } from 'primeng/primeng';

import { PatchGraphDocument } from './../../../../../infrastructure/services/patchGraphDocument';
import { JsonSchema, SchemaFormBuilder } from './../../../../../infrastructure/services/schema';

import { UserService } from './../user.service';
import { User, UserApiService } from '../user-api.service';

@Component({
    selector: 'dualog-user-details',
    templateUrl: './user-details.component.html',
    styleUrls: [ './user-details.component.scss' ]
})

export class UserDetailsComponent {

    msgs: Message[] = [];
    // _user: User;

    constructor( public userService: UserService ) {
    }


    // @Input()
    // get user(): User {
    //     return this._user;
    // }
    // set user(value: User) {
    //     this._user = value;
    //     this.loadUserDetails();
    // }




}
