import { Component, Input, OnInit } from '@angular/core';
import { FormGroup, FormArray } from '@angular/forms';
import { Permission, PermissionService } from 'connection-suite-shore/services/permission.service';
import { SchemaFormBuilder, JsonSchema } from 'dualog-common';
import { Observable } from 'rxjs/Observable';
import { SelectItem, MenuItem } from 'primeng/primeng';
import { SchemaFormGroup, SchemaFormArray } from 'dualog-common/schema';
import { UserService } from '../user.service';

@Component({
    selector: 'dualog-user-permission-list',
    templateUrl: './user-permission-list.component.html',
    styleUrls: [ './user-permission-list.component.scss' ]
})
export class UserPermissionListComponent
       implements OnInit {

    _permissions: FormArray;
    availablePermissions: SelectItem[];
    access: SelectItem[];
    rowCommands: MenuItem[];
    isInNewMode = false;

    displayDialog = false;
    permission = {};

    @Input() schema: Observable<JsonSchema>;

    constructor( private userService: UserService, private permissionService: PermissionService, private fb: SchemaFormBuilder ) {
        this.rowCommands = [
            // { label: 'Save', icon: 'fa-check', command: () => { this.saveRow(); } },
            { label: 'Cancel', icon: 'fa-check', command: () => { this.cancelRow(); } }
        ];

        this.access = [
            { label: 'None', value: 'None' },
            { label: 'Read', value: 'Read' },
            { label: 'Write', value: 'Write' },
        ];
    }

    @Input()
    get permissions(): FormArray {
        return this._permissions;
    }
    set permissions(value: FormArray) {
        this._permissions = value;
    }

    public ngOnInit() {
        this.permissionService.getPermissions()
            .map( p =>
                p.map( item => {
                    return { label: item.name, value: item.name };
                }) )
            .subscribe( p => {
                this.availablePermissions = p;
            } );
    }

    public createNewPermission(): void {

        this.permission = {
            name: null,
            allowType: null,
            origin: null
        }

        this.displayDialog = true;
        this.isInNewMode = true;
    }

    public savePermission(): void {

        const d = {
            permissions: [ this.permission ]
        };
        const fg = this.fb.ReactiveBuild( this.schema, Observable.of( d ) ).subscribe( n => {

            const pa = <FormArray> n.get('permissions');
            this.permissions.push(pa.controls[0]);
        });

        this.isInNewMode = false;
        this.displayDialog = false;
    }

    public cancelRow(): void {
        this.isInNewMode = false;
    }


    // This function handles allowtype changes
    public allowTypeChanged(permission, event): void {

        const control = <SchemaFormGroup> this.permissions.controls.find( item => item.get('name').value === permission.name );
        if (!control) {
            return;
        }

        const ctrlAllowType = control.get( 'allowType' )
        ctrlAllowType.setValue( event.value );

        if (event.value === 'None') {
            const sfa = <SchemaFormArray> control.parent;
            sfa.markAsDeleted( control );
        } else {
            ctrlAllowType.markAsDirty( {onlySelf: false } );
        }
    }
}
