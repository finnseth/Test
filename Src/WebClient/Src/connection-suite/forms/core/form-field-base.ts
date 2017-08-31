import { ControlValueAccessor, FormGroupDirective, NgControl } from '@angular/forms';
import { AfterContentInit, Injector, Input, OnDestroy } from '@angular/core';

export abstract class FormFieldBase implements AfterContentInit, OnDestroy {
    private _fc: any;
    private _caption: string;
    private _description: string;
    private _ngControl: NgControl;
    private _disabled = false;
    private _required = false;
    private _focused = false;
    private _validationText: string;

    /** Gets or sets wheither the input is disabled */
    @Input()
    get disabled(){ return this._ngControl ? this._ngControl.disabled : this._disabled; }
    set disabled( value: any ){ this._disabled = value; }

    // Gets or sets the caption
    @Input()
    public get caption(){ return this._caption; }
    public set caption( caption: string ){ this._caption = caption; }

    /** Gets or sets the description of the input */
    @Input()
    get description(){ return this._description; }
    set description( value: string ) { this._description = value; }

    /** Gets or sets wheither a value is required */
    @Input()
    get required(){ return this._required; }
    set required( value: boolean ) { this._required = value; }

    /** Gets or sets a value indicating wheither the control is focused or not */
    @Input()
    get focused(): boolean{ return this._focused; }
    set focused( value: boolean ) { this._focused = value; }

    /** Gets or sets the validation text */
    protected get validationText(): string { return this._validationText; }
    protected set validationText( value: string ) { this._validationText = value; }


    protected get fc(): any { return this._fc; }

    /** View -> model callback called when value changes */
    private onControlValueChanged: (value: any) => void = (value) => {};

    /** View -> model callback called when select has been touched */
    protected onTouched: () => any = () => {};

    constructor(protected injector: Injector) {}


    protected AfterContentInit(): void {
        this._ngControl = this.injector.get(NgControl);
        const fgd: FormGroupDirective = this._ngControl['_parent'];
        this._fc = fgd.control.controls[this._ngControl.name];

        if ( this.caption === undefined ) {
            this.caption = this._fc['__title'];
        }

        if ( this.description === undefined ) {
            this.description = this._fc['__description'];
        }
    }

    protected OnDestroy(): void {
    }

    /**
     * Sets the value of the component.
     * @param value The value to set
     */
    protected abstract internalWriteValue(value: any);


    /**
     * Sets the select's value. Part of the ControlValueAccessor interface
     * required to integrate with Angular's core forms API.
     *
     * @param value New value to be written to the model.
     */
    writeValue(value: any): void {
        this.internalWriteValue(value);
    }

   /**
   * Saves a callback function to be invoked when the select's value
   * changes from user input. Part of the ControlValueAccessor interface
   * required to integrate with Angular's core forms API.
   *
   * @param fn Callback to be triggered when the value changes.
   */
    registerOnChange(fn: any): void {
        this.onControlValueChanged = fn;
    }

   /**
   * Saves a callback function to be invoked when the select is blurred
   * by the user. Part of the ControlValueAccessor interface required
   * to integrate with Angular's core forms API.
   *
   * @param fn Callback to be triggered when the component has been touched.
   */
    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

   /**
   * Disables the select. Part of the ControlValueAccessor interface required
   * to integrate with Angular's core forms API.
   *
   * @param isDisabled Sets whether the component is disabled.
   */
    setDisabledState(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    /** Called after a components content has been initialized.  */
    ngAfterContentInit(): void {
        this.AfterContentInit();
    }

    /** Called just before the component is destroyed. */
    ngOnDestroy(): void {
        this.OnDestroy();
    }

    /** Call this to change the value. */
    protected onValueChanged(value: any) {
        this.onControlValueChanged(value);
        this.checkValidity();
    }

    /** Validates the value of the control agains the information in the schema, if such exists. */
    private checkValidity() {
        if ( !this._ngControl || this._ngControl.valid === true ) {
            this._validationText = null;
            return;
        }

        const validatorInfo = this._fc['__validatorInfo'];
        if ( validatorInfo === undefined ) {
            return;
        }

        for (const errorText in this._ngControl.errors) {

            if ( this._ngControl.hasError( errorText ) === false ) {
                continue;
            }

            this.validationText = this.validate(errorText, validatorInfo);
        }
    }

    /** Default validation */
    protected validate( errorText: string, validatorInfo: any ): string {
        switch ( errorText ) {
            case 'required':
                return `${this.caption} is required.`;

            case 'max':
                return `${this.caption} cannot be greater than ${validatorInfo.maximum}.`;

            case 'maxlength':
                return `${this.caption} cannot be more than ${validatorInfo.maxLength} characters.`;

            case 'min':
                return `${this.caption} cannot be lesser than ${validatorInfo.minimum}.`;

            case 'minlength':
                return `${this.caption} cannot be lesser than ${validatorInfo.minLength} characters.`;
        }
    }
}
