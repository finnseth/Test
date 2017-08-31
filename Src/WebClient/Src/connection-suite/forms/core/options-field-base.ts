import {
    AfterContentInit,
    ChangeDetectorRef,
    ContentChildren,
    Injector,
    Input,
    OnDestroy,
    QueryList
} from '@angular/core';
import { Observable, Subscription } from 'rxjs/Rx';
import { CsOptionComponent, CsOptionEvent } from '../option';
import { FormFieldBase } from './form-field-base';

export abstract class OptionsFieldBase extends FormFieldBase {

    private _values: any[];
    private _options: QueryList<CsOptionComponent>;
    private _optionsSubscription: Subscription;
    private _changeSubscription: Subscription;

    private _disableUpdates = false;

    /** Gets or sets wheiher multiple options can be selected  */
    protected abstract get isMultiple(): boolean;

    /** The options for the componewnt. */
     @ContentChildren(CsOptionComponent)
     get options(): QueryList<CsOptionComponent> { return this._options; }
     set options( val: QueryList<CsOptionComponent> ) { this._options = val; }

    /** The values for the compoonent */
    @Input()
    public get values(): any[]{
        return this._values ? this._values : new Array<any>();
    }
    public set values( value: any[] ) {
        this.setValue( value );
        super.onValueChanged( this.value );
    }

    /** Gets the value */
    public get value(): any {
        if (!this._values || this._values.length === 0) {
            return;
        } else if ( this._values.length === 1) {
            return this._values[0];
        } else {
            return this._values;
        }
    }

    /** Creates a new instance of the OptionsFieldBase class */
    constructor( protected _changeDetectorRef: ChangeDetectorRef, _injector: Injector) {
        super( _injector );
    }

    /** Sets all options to not selected */
    public resetOptions(): void {
        this.options.forEach( o => {
            o.selected = false;
        });
    }

    protected internalWriteValue(value: any) {
        this.setValue(value);
        this.onTouched();
    }


    protected AfterContentInit(): void {
        this._changeSubscription = this.options.changes.startWith(null).subscribe(() => {
            this.subscribeToOptions();
        } );

       // Tries to sets the value if it is set
        if (this.values && this.options) {

            this.updateOptions();
        }

        this.updateValues( );
        super.AfterContentInit();
    }

    protected OnDestroy(): void {
        super.OnDestroy();

        this._changeSubscription.unsubscribe();
        this._optionsSubscription.unsubscribe();
    }

    /** Gets the options (child) of this select by value */
    private findOptionByValue( value: any ): CsOptionComponent {

        if (!this._options) {
            return;
        }

        return this._options.find( (o, i, a) => {
            return o.value === value;
        });
    }

    /** Subscripes to selection changes on registered options.  */
    private subscribeToOptions(): void {

        const om =  <Observable<CsOptionEvent>> Observable.merge( ...this.options.map(option => option.onSelectedChange) );
        this._optionsSubscription = om.subscribe( event => {
            if (this._disableUpdates === false) {
                this.updateValues();
            }
        } );
    }

    /** Get the values based on selected options */
    private updateValues() {

        const nv = new Array<any>();

        this.options.forEach( o => {
            if (o.selected) {
                nv.push(o.value);
            }
        });

        this.values = nv;
        this._changeDetectorRef.markForCheck();
    }

    /** Updates the options based on the  values */
    private updateOptions() {
        this._values.forEach( val => {
            const option = this.findOptionByValue(val);
            if (option) {
                option.selected = true;            }

        });

        this._changeDetectorRef.markForCheck();
    }

    /** Sets the value */
    private setValue( value: any ) {

        if (!value){
            this._values = new Array<any>();
            return;
        }

        const isArray = Array.isArray(value);

        if (isArray === true) {
            this._values = value;
        } else {
            this._values = new Array();
            this._values[0] = value;
        }
    }

    /** Triggers when a value is selected */
    private onValueSelected( value: any ) {
        const option = this.findOptionByValue(value);
        if (option) {
            if (this.value) {
                const oldOption = this.findOptionByValue(this.value);
                oldOption.selected = false;
            }

            option.selected = !option.selected;
            this.updateValues();
        }
    }
}
