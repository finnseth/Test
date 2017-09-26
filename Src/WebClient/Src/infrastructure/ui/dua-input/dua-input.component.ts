import { ChangeDetectorRef, Component, Input, ViewChild, forwardRef, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR, FormGroup } from '@angular/forms';

export const INPUT_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DuaInputComponent),
  multi: true
};

@Component({
  selector: 'dua-input',
  styleUrls: ['./dua-input.component.scss'],
  templateUrl: './dua-input.component.html',
  providers: [INPUT_VALUE_ACCESSOR]
})

export class DuaInputComponent implements ControlValueAccessor, OnInit {

    @Input() formControlName: string;
    @Input() parentFormGroup: FormGroup;

    innerValue = '';
    isRequired = false;

    constructor() {}

    ngOnInit() {
        if (this.parentFormGroup.controls[this.formControlName]['__required'] !== undefined) {
            this.isRequired = this.parentFormGroup.controls[this.formControlName]['__required'];
        }
    }

    // Placeholders for the callbacks which are later providesd by the Control Value Accessor
    onTouchedCallback(): void { }

    onChangeCallback(_: any): void { }

    get value(): any {
        return this.innerValue;
    };

    set value(v: any) {
        if (v !== this.innerValue) {
            this.innerValue = v;
            this.onChangeCallback(v);
        }
    }

    onBlur() {
        this.onTouchedCallback();
    }

    // From ControlValueAccessor interface
    writeValue(value: any) {
        if (value !== this.innerValue) {
            this.innerValue = value;
        }
    }

    // From ControlValueAccessor interface
    registerOnChange(fn: any) {
        this.onChangeCallback = fn;
    }

    // From ControlValueAccessor interface
    registerOnTouched(fn: any) {
        this.onTouchedCallback = fn;
    }
}
