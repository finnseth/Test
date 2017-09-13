import { AbstractControl, AsyncValidatorFn, FormArray, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';

import { CustomValidators } from 'ng2-validation';
import { Injectable } from '@angular/core';
import { JsonPatchOperation } from '../';
import { JsonSchema } from './json-schema';
import { Observable } from 'rxjs/Rx';

export class SchemaFormControl extends FormControl {
    constructor(
        formState?: any,
        validator?: ValidatorFn | ValidatorFn[],
        asyncValidator?: AsyncValidatorFn | AsyncValidatorFn[] ) {

        super( formState, validator, asyncValidator )
    }

    public afterInit(): void {
    }

     public static createFormControl( schema: JsonSchema): SchemaFormControl {
        const validatorInfo = {};
        const fc = new SchemaFormControl( null, this.getValidator( schema, validatorInfo ));

        this.setInputType( fc, schema );
        fc['__validatorInfo'] = validatorInfo;
        fc['__title'] = schema.title || '';
        fc['__description'] = schema.description || '';

        return fc;
    }

    /** Creates the properties validators from the provided schema. */
    public static getValidator( prop: JsonSchema,  validatorInfo: any ): any[] {

        let validators = new Array<any>();

        // Check wheither the property is required
        if ( prop['isRequired']  ) {
            validators.push( Validators.required );
        }

        if ( this.isPropertyOfType( prop, 'string' ) ) {
            validators = validators.concat( validators, this.getStringValidators( prop, validatorInfo ) );
        } else if ( this.isPropertyOfType( prop, 'integer' ) ) {
            validators = validators.concat( validators, this.getNumberValidators( prop, validatorInfo ) );
        }

        return validators;
    }

    /** Creates the string specific validators from the provided schema  */
    static getStringValidators( schema: JsonSchema, validatorInfo: any ): any[] {

        const validators = new Array<any>();

        if ( schema === undefined ) {
            return validators;
        }

        // Add min length, if neccessary
        const minLength = schema['minLength'];
        if ( minLength !== undefined && minLength > 0 ) {

            validators.push( Validators.minLength( minLength ));
            validatorInfo['minLength'] = minLength;
        }

        // Add max length, if neccessary
        const maxLength = schema['maxLength'];
        if ( maxLength !== undefined && maxLength > 0 ) {

            validators.push( Validators.maxLength( maxLength ));
            validatorInfo['maxLength'] = maxLength;
        }

        return validators;
    }

    /** Creates the number specific validators from the provided schema. */
    static getNumberValidators( schema: JsonSchema, validatorInfo: any ): any[] {

        const validators = new Array<any>();

        if ( schema === undefined ) {
            return validators;
        }

        const minimum = schema['minimum'];
        if ( minimum !== undefined ) {
            validators.push( CustomValidators.min( minimum ) );
            validatorInfo['minimum'] = minimum;
        }

        const maximum = schema['maximum'];
        if ( maximum !== undefined ) {
            validators.push( CustomValidators.max( maximum ) );
            validatorInfo['maximum'] = maximum;
        }

        return validators;
    }

    /** Gets wheiter, from the provided schema, is of the givven type,  */
    static isPropertyOfType( schema: JsonSchema, propertyType: string ): boolean {

        const pType = schema.type;
        let result: boolean;


        if ( pType instanceof Array) {
            result =  pType.indexOf( propertyType ) > -1;
        } else {
            result = pType === propertyType;
        }

        return result;
    }

    /** Set the input type from the schema */
    static setInputType( fc: FormControl, schema: JsonSchema ): void {

        if ( this.isPropertyOfType( schema, 'string' )) {
            fc[ '__type' ] = 'text';

        } else if ( this.isPropertyOfType( schema, 'integer' )) {
            fc[ '__type' ] = 'number';
        }
    }
}

export class SchemaFormGroup extends FormGroup  {
    keyProperty: string;

    public Parse( obj: any ) {

        for (const property in obj) {
            const value = obj[property];

            const control = this.controls[ property ];
            if (!control) {
                continue;
            }

            if ( control instanceof SchemaFormArray  ) {
                (<SchemaFormArray> control).Parse( value );
            } else {
                control.setValue( value );
            }
        }
    }

    public afterInit(): void {

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

}

export class SchemaFormArray extends FormArray {

    _schema: JsonSchema;
    _schemaFormBuilder: SchemaFormBuilder;

    _deleted: any[] = [];
    _added: any[] = [];


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
        const val = fg.controls[fg.keyProperty].value;

        this._deleted.push(val);
        super.removeAt(index);

        super.markAsDirty({ onlySelf: true });
    }

    public push( control: AbstractControl): void {

        if (control instanceof SchemaFormGroup) {

            this._added.push( control );
            super.markAsDirty({ onlySelf: true });
        }

        // add the control to the form array
        super.push( control );
    }
}




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
