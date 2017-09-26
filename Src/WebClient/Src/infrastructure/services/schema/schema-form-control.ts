import { FormControl, ValidatorFn, AsyncValidatorFn, Validators } from '@angular/forms';
import { SchemaFormGroup } from './schema-form-group';
import { SchemaFormArray } from './schema-form-array';
import { JsonSchema } from './json-schema';
import { CustomValidators } from 'ng2-validation/dist';


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
        fc['__required'] = schema['isRequired'];

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
