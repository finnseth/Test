import { Component, Input, EventEmitter, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { User, UserApiService } from '../user-api.service';
import { SchemaFormBuilder, JsonSchema } from 'dualog-common';
import { PatchGraphDocument } from 'dualog-common/services/patchGraphDocument';
import { Message } from 'primeng/primeng';
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
