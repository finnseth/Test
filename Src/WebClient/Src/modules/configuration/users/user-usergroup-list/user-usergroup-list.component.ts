import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormArray } from '@angular/forms';
import { Header, Footer } from 'primeng/primeng';
import { UserGroupService } from 'connection-suite-shore/services/userGroup.service';
import { JsonSchema, SchemaFormBuilder } from 'dualog-common';
import { Observable } from 'rxjs/Rx';
import { SchemaFormArray } from 'dualog-common/schema/schema-form-array';

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
