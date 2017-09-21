import { Component, Input, Output, EventEmitter } from '@angular/core';

import { User } from '../user-api.service';

@Component({
    selector: 'dualog-user-list-item',
    templateUrl: './user-list-item.component.html',
    styleUrls: [ './user-list-item.component.scss' ]
})

export class UserListItemComponent {

    @Input()
    user: User;

    @Output()
    userSelected = new EventEmitter();

    userClicked() {
        this.userSelected.emit();
    }
}
