import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, FormControl, ValidatorFn, AsyncValidatorFn, AbstractControl } from '@angular/forms';
import { Observable } from 'rxjs/Rx';
import { CustomValidators } from 'ng2-validation';

import { JsonSchema } from './json-schema';
import { SchemaFormGroup } from './schema-form-group';
import { SchemaFormArray } from './schema-form-array';
import { SchemaFormControl } from './schema-form-control';





@Injectable()
export class SchemaFormBuilder {
    /** Creates a new instance of the SchemaFormBuilder class.  */
    constructor( private fb: FormBuilder ) {
    }

    /**
     * Builds a new FormGroup from the givven schema and data object
     * @param jsonSchema The schema to be used when creating the form
     * @param data the data to be used to fill the form
     */
    public ReactiveBuild( jsonSchema: Observable<JsonSchema>, data: Observable<any> ): Observable<SchemaFormGroup> {
        return Observable.zip(
            jsonSchema,
            data,
            (schema, obj) => {
                return this.Build( schema, obj );
            } );
    }

    public Build(  jsonSchema: JsonSchema, data: any ): SchemaFormGroup {
        const fg = this.BuildForm( jsonSchema );
        fg.Parse( data );
        fg.afterInit();
        fg.markAsPristine();
        fg.markAsUntouched();

        return fg;
    }


    public BuildForm(schema: JsonSchema): SchemaFormGroup {

        const fg = new SchemaFormGroup({});

        for ( const property in schema.properties ) {

            const schemaProperty = schema.properties[property];
            const type = (<string[]> schemaProperty.type);

            if (property.toLocaleLowerCase() === 'id' && !fg.keyProperty ) {
                fg.keyProperty = property;
            }

            const key = schemaProperty['key'];
            if (key && key === true) {
                fg.keyProperty = property;
            }

            // Check if this is an array
            if ( type && type.includes( 'array')) {

                let ds: string = schemaProperty.items['$ref'];
                ds = ds.substring( ds.lastIndexOf('/') + 1 );
                const definition = schema.definitions[ ds ];

                const fga = new SchemaFormArray([]);

                if (definition) {
                    fga.schema = definition;
                    fga._schemaFormBuilder = this;
                }

                fg.addControl( property, fga);

            } else {

                const fc = SchemaFormControl.createFormControl( schemaProperty );
                fg.addControl( property, fc );
            }
        }

        return fg;
    }
}
