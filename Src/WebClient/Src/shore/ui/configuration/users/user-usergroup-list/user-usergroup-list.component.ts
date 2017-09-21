import { Component, Input, OnInit } from '@angular/core';
import { Footer, Header } from 'primeng/primeng';
import { FormArray, FormGroup } from '@angular/forms';

import { Observable } from 'rxjs/Rx';

import { JsonSchema, SchemaFormBuilder } from './../../../../../infrastructure/services/schema';
import { SchemaFormArray } from './../../../../../infrastructure/services/schema/schema-form-array';

import { UserGroupService } from './../../../../services/userGroup.service';

@Component({
    selector: 'dualog-user-usergroup-list',
    templateUrl: './user-usergroup-list.component.html',
    styleUrls: [ './user-usergroup-list.component.scss' ]
})
export class UserUserGroupComponent implements OnInit {


    @Input() schema: Observable<JsonSchema>;
    @Input() userGroups: FormArray;

    availableUserGroups: Observable<FormGroup[]>;
    displayUsergroupDialog = false;

    constructor( private userGroupService: UserGroupService, private fb: SchemaFormBuilder) {
    }

    public removeUserGroups(event) {

        const fga = <SchemaFormArray> this.userGroups;

        event.items.forEach(item => {
            fga.markAsDeleted(item);
        });
    }

    public addUserGroups(event) {

        const fga = <SchemaFormArray> this.userGroups;
        event.items.forEach( item => {
            fga.addReference( item );
        });
    }

    ngOnInit(): void {
        // Get all usergroups from the API
        const ugs = this.userGroupService.getAll().map( ug => {
            return { userGroups: ug };
        });

        // combine with schema to create a formgroup
         this.availableUserGroups = this.fb.ReactiveBuild( this.schema, ugs ).map( s => {

            // Get the user groups from the returned result
            const ugArray = <FormArray> s.get('userGroups');
            const userGroups = <FormGroup[]> ugArray.controls

            // Filter out items that exists in available items
            return userGroups.filter( f =>
                        !this.userGroups.controls.some( item =>
                            item.get( 'name' ).value === f.get( 'name' ).value ) ) ;
        } );
    }
}
