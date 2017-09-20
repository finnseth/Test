import { Component, EventEmitter, Input, Output } from '@angular/core';
import { JsonSchema, SchemaFormBuilder } from 'infrastructure/services/schema';
import { User, UserApiService } from '../user-api.service';

import { FormGroup } from '@angular/forms';
import { Message } from 'primeng/primeng';
import { Observable } from 'rxjs/Rx';
import { PatchGraphDocument } from 'infrastructure/services/patchGraphDocument';
import { UserService } from 'modules/configuration/users/user.service';

@Component({
    selector: 'dualog-user-details',
    templateUrl: './user-details.component.html',
    styleUrls: [ './user-details.component.scss' ]
})

export class UserDetailsComponent {

    msgs: Message[] = [];
    // _user: User;

    constructor( protected userService: UserService ) {
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
