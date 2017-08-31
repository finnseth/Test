import { ChangeDetectorRef, Injector, Input } from '@angular/core';
import { FormFieldBase } from './form-field-base';

export abstract class SingleFieldBase extends FormFieldBase {

    protected _value: any;

    // Gets or sets the value
    @Input()
    public get value(){ return this._value; }
    public set value( value: any ){ this._value = value; }

     constructor( protected _changeDetectorRef: ChangeDetectorRef, injector: Injector ) {
         super( injector );
     }


    protected internalWriteValue(value: any) {
        this.value = value;
        this.onTouched();
    }

}
