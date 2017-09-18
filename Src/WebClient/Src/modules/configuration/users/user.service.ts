import { Injectable } from '@angular/core';
import { UserApiService, User, UserDetail } from './user-api.service';
import { Observable, Subject } from 'rxjs/Rx';
import { FormGroup } from '@angular/forms';
import { SchemaFormBuilder, JsonSchema } from 'dualog-common';
import { PatchGraphDocument } from 'dualog-common/services/patchGraphDocument';


@Injectable()
export class UserService {

    schema: Observable<JsonSchema>;
    _currentUser: User = null;
    userDetails: UserDetail;
    userForm: FormGroup;

    canSaveUser = false;



    get currentUser(): User {
        return this._currentUser;
    }
    set currentUser(value: User) {
        this._currentUser = value;
        this.loadUserDetails();
    }

    constructor( private userApiService: UserApiService, private fb: SchemaFormBuilder ) {
        this.schema = this.userApiService.GetUserSchema();
    }

    private loadUserDetails() {
        if (!this.currentUser) {
            return;
        }

        this.fb.ReactiveBuild(this.schema, this.userApiService.GetUser( this.currentUser.id )).subscribe( uf => {
            this.userForm = uf;
        });
    }

    saveUser() {
        if (this.userForm.valid === false) {
            console.log( 'The form is invalid.' );
            // this.msgs.push({severity: 'error', summary: 'Error', detail: 'The form data is invalid.'})
            return;
        }

        if (this.userForm.pristine === true) {
            console.log( 'no changes.' );
            // this.msgs.push({severity: 'warn', summary: 'Warning', detail: 'There are no changes on the form.'})
            return;
        }

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument( this.userForm );

        const obsPatch = this.userApiService.PatchUserById( this.currentUser.id, JSON.stringify(jsonPatch) ).share();
        obsPatch.subscribe( result => {

            this.userForm.markAsPristine();
            this.updateListItem( this.userForm );
        } );

        this.fb.ReactiveBuild(this.schema, obsPatch).subscribe( f => {
           this.userForm = f;
        });

    }

    private updateListItem( userForm: FormGroup ) {

        this.currentUser.email = userForm.controls['email'].value;
        this.currentUser.name = userForm.controls['name'].value;
    }
}
