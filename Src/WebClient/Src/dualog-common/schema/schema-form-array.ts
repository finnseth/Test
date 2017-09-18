import { FormArray, AbstractControl } from '@angular/forms';
import { JsonSchema } from './json-schema';
import { SchemaFormGroup } from './schema-form-group';
import { SchemaFormControl } from './schema-form-control';
import { SchemaFormBuilder } from './schema-form-builder';

export class SchemaFormArray extends FormArray {

    _schema: JsonSchema;
    _schemaFormBuilder: SchemaFormBuilder;

    _deleted: any[] = [];
    _added: any[] = [];
    _referenced: any[] = [];


    public set schema( value: JsonSchema ) { this._schema = value; }
    public get schema(): JsonSchema { return this._schema; }

    public afterInit(): void {

        this._added = [];
        this._deleted = [];

        for (const property in this.controls) {

            const c = this.controls[property];
            if (c instanceof SchemaFormArray) {
                (<SchemaFormArray> c).afterInit();
            } else if (c instanceof SchemaFormGroup) {
                (<SchemaFormGroup> c).afterInit();
            } else if (c instanceof SchemaFormControl) {
                (<SchemaFormControl> c).afterInit();
            }
        }
    }

    public Parse( values: any[] ) {

        values.forEach( o => {
            const fc = this._schemaFormBuilder.Build( this._schema, o );
            this.push( fc );
        });
    }

    public removeAt( index: number ): void {

        const fg = <SchemaFormGroup> this.controls[index];
        const val = SchemaFormGroup.GetKeyValue<any>( fg );

        this._deleted.push(val);
        super.removeAt(index);

        super.markAsDirty({ onlySelf: true });
    }


    public push( control: AbstractControl, addToArray = true ): void {

        if (control instanceof SchemaFormGroup) {

            this.removeFromDeleted(control);
            this._added.push( control );

            control.markAsDirty({ onlySelf: false });
            super.markAsDirty({ onlySelf: false });
        }

        // add the control to the form array
        if (addToArray) {
            super.push( control );
        }
    }

    public pushAll( items: AbstractControl[], addToArray = true ): void {

        items.forEach( i => {
            this.push( i, addToArray );
        });
    }

    public markAsDeleted(control: SchemaFormGroup): void {
        this.removeFromAdded(control);

        this._deleted.push( SchemaFormGroup.GetKeyValue(control) );
        super.markAsDirty();
    }

    public addReference( control: SchemaFormGroup ): void {

        const key = control.keyProperty;
        const value = SchemaFormGroup.GetKeyValue(control);
        const o = {};
        o[key] = value;

        this._referenced.push( o );
        super.markAsDirty();
    }

    private removeFromAdded( control: SchemaFormGroup ): void {
        const val = SchemaFormGroup.GetKeyValue(control);
        const index = this._added.findIndex( item => {
            const v2 = item.controls[item.keyProperty].value;
            return val === v2;
        } );

        if ( index >= 0) {
            this._added.splice( index, 1 );
        }
    }

    private removeFromDeleted( control: SchemaFormGroup ): void {
        const val = SchemaFormGroup.GetKeyValue(control);
        const index = this._deleted.findIndex( item => {
            const v2 = item.controls[item.keyProperty].value;
            return val === v2;
        } );

        if ( index >= 0) {
            this._deleted.splice( index, 1 );
        }
    }
}
