import { ActivatedRoute, Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { Message, TreeNode, SelectItem } from 'primeng/primeng';
import { User, UserDetail, UserService } from '../user.service';

import { FormGroup } from '@angular/forms/forms';
import { JsonSchema } from 'dualog-common';
import { Observable } from 'rxjs/Rx';
import { SchemaFormBuilder } from 'dualog-common';
import { Ship } from 'connection-suite/components/ship/interfaces';
import { SchemaFormArray, SchemaFormGroup, SchemaFormControl } from 'dualog-common/services/schema-form-builder.service';
import { FormControl } from '@angular/forms/src/forms';
import { PatchGraphDocument } from 'dualog-common/services/patchGraphDocument';

@Component({
    templateUrl: './list-users.component.html',
    styleUrls: [ './list-users.component.css' ]
})
export class ListUsersComponent implements OnInit {

    schema: JsonSchema;
    leftForm: Observable<FormGroup>;
    shipForm: Observable<FormGroup>;
    shipUserForm: Observable<FormGroup>;
    userForms: FormGroup[] = [];
    fleetusers: User[];
    shipusers: User[];
    leftusers: User[];
    shipUser: UserDetail;
    selectedShip: Ship;
    selectedCompareShip: Ship;
    gotCompareShip = false;

    editShip?: number;



    accessRights: SelectItem[] = [];

    constructor( private userService: UserService, private fb: SchemaFormBuilder ) {
        this.accessRights.push({label: 'Read', value: 'Read'});
        this.accessRights.push({label: 'Write', value: 'Write'});
    }

    ngOnInit() {

        this.schema = this.userService.GetUserSchema().subscribe( s => {
            this.schema = s;

            this.userService.GetUsers().subscribe( u => {
                this.fleetusers = u;
                this.leftusers = this.fleetusers;
                this.leftForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftusers ) );
            });
        });
    }

    getUserGroups( values: SchemaFormArray ) {


        return values.controls.map( ug => {
            const sfg = ug as SchemaFormGroup;
            return {
                name: sfg.controls.name.value
            }
        });
    }

    getPermissions( values: SchemaFormArray ) {
        return values.controls.map( ug => {
            const sfg = ug as SchemaFormGroup;
            return {
                name: sfg.controls.name.value,
                allowType: sfg.controls.allowType.value,
                origin: sfg.controls.origin.value
            }
        });
    }

    removePermission(userId: number, ug: any) {

        const c = this.userForms[userId]
        const permissions = <SchemaFormArray> c.controls.permissions;
        const index = permissions.controls.findIndex( i => {

            const sfg =  <SchemaFormGroup> i;
            if (sfg.controls.origin.value === 'User' && sfg.controls.name.value === ug.name ) {
                return true;
            }

            return false;
        } );

        permissions.removeAt( index );
    }

    editUser(event) {
        this.editShip = event.data.id;
    }

    usergroupExpand(event) {
        if (event.node) {
            // get user group permissions
            // get user group method filter ++
            // in a real application, make a call to a remote url to load children of the current node and add the new nodes as children
            // this.nodeService.getLazyFilesystem().then(nodes => event.node.children = nodes);
        }
        console.log('@todo add user group change');
    }

    selectShipUser(user: User) {
         this.userService.GetUser(user.id).subscribe( u => {
            this.shipUser = u;
            this.userForms[this.shipUser.id] = this.fb.Build(this.schema, this.shipUser);
        });
    }

    // @todo need api call
    handleShipChange(ship: Ship) {
        if ( ship !== null ) {
            this.userService.GetUsers().subscribe( r => {
                this.shipusers = r;
                this.shipForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.shipusers ) );
            });
        }
    }

    // @todo need api call
    handleCompareChange(ship: Ship) {
        if ( ship !== null ) {
            this.userService.GetUsers().subscribe(r => {
                this.leftusers = r;
                this.leftForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftusers ) );
            });
        }
    }


    showFleet(event: boolean) {
        this.gotCompareShip = false;
        this.leftusers = this.fleetusers;
        this.leftForm = this.fb.ReactiveBuild( Observable.of( this.schema ), Observable.of( this.leftusers ) );
    }

    submitForm(fg: FormGroup): void {
/*
        if (fg.valid === false) {
            console.log( 'The form is invalid.' );
            this.msgs.push({severity: 'error', summary: 'Error', detail: 'The form data is invalid.'})
            return;
        }

        const jsonPatch = SchemaFormBuilder.GetPatchData( fg );
        if (jsonPatch !== undefined && jsonPatch.length > 0) {

            console.log(JSON.stringify(jsonPatch));
            fg.markAsPristine();
            this.userService.PatchUserById( this.currentUserId, JSON.stringify(jsonPatch) ).subscribe( result => {

                fg.markAsPristine();
            } );
        }
*/
    }

    saveUser(userForm: SchemaFormGroup): void {

        const pgd = new PatchGraphDocument();
        const jsonPatch = pgd.CreatePatchDocument( userForm );

        if (jsonPatch) {

        }
    }

    newRule(event: Event ) {
        console.log('@todo: add new rule');
    }

    cancelRuleChanges(event: Event ) {
        console.log('@todo: add cancel rule');
    }

    applyRuleChanges(event: Event ) {
        console.log('@todo: add apply rule');
    }
}
