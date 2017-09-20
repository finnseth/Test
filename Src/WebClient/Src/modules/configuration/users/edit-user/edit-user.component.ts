import { ActivatedRoute, Router } from '@angular/router';
import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormGroup } from '@angular/forms';
import { JsonSchema, SchemaFormBuilder } from 'infrastructure/services/schema';
import { User, UserApiService, UserGroup } from '../user-api.service';

import { Observable } from 'rxjs/Rx';
import { PatchGraphDocument } from 'infrastructure/services/patchGraphDocument';
import { SelectItem } from 'primeng/primeng';

@Component({
    selector: 'edit-user',
    templateUrl: './edit-user.component.html',
})
export class EditUserComponent {

    _id?: number;

    @Input()
    public get id(): number{
        return this._id;
    }
    public set id(value: number) {
        this._id = value;
        this.loadData();
    }

    userForm: Observable<FormGroup>;
    existingUserGroups: Observable<SelectItem[]>;
    permissions: Observable<SelectItem[]>;
    userGroups: Observable<UserGroup[]> = Observable.empty();


    constructor( private route: ActivatedRoute,
                 private router: Router,
                 private userApiService: UserApiService,
                 private fb: SchemaFormBuilder ) {
    }

    handleUserGroupChange(event) {
    }

    submitForm( fg: FormGroup ) {

        if (fg.valid === false) {
            console.log( 'The form is invalid.' );
            return;
        }

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument(  fg );
        if (jsonPatch ) {

            this.userApiService.PatchUserById( this.id, jsonPatch ).subscribe( result => {

                fg.markAsPristine();
            } );
        }
    }

    loadData(): void {
        const schema = this.userApiService.GetUserSchema().share();
        const user = this.userApiService.GetUser( this.id );
        this.userForm = this.fb.ReactiveBuild( schema, user );
    }

   getUserGroupSuggestions(event) {

        if (!this.userGroups) {
            this.userGroups = this.userApiService.GetUserGroups();
        }

        this.userGroups = this.userGroups.map( ug => {

            return ug.filter( item => {

                return item;
            })
        } );
   }

    userGroupSelected(event, fg: FormGroup) {
    }

}
