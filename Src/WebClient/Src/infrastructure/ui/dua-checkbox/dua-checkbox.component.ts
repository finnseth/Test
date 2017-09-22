import { ChangeDetectorRef, Component, EventEmitter, Input, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';

import { Checkbox } from 'primeng/primeng';

export const CHECKBOX_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DuaCheckboxComponent),
  multi: true
};

@Component({
  selector: 'dua-checkbox',
  styleUrls: ['./dua-checkbox.component.scss'],
  templateUrl: './dua-checkbox.component.html',
  providers: [CHECKBOX_VALUE_ACCESSOR]
})

export class DuaCheckboxComponent extends Checkbox implements ControlValueAccessor {

    @Input() value: any;
    @Input() name: string;
    @Input() binary: string;
    @Input() label: string;
    @Input() tabindex: number;
    @Input() inputId: string;
    @Input() style: any;
    @Input() styleClass: string;
    @Input() formControlName: string;
    @Input() parentFormGroup: string;
    @Input() tooltip: string;

    @Input() compareStyle = false;

    @Output() onChange: EventEmitter<any> = new EventEmitter();

    constructor(cd: ChangeDetectorRef) {
        super(cd);
    }
}
