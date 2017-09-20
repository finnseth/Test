import { ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, Output, forwardRef } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { DomHandler, Spinner } from 'primeng/primeng';

export const SPINNER_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => DuaSpinnerComponent),
  multi: true
};

@Component({
  selector: 'dua-spinner',
  styleUrls: ['./dua-spinner.component.scss'],
  templateUrl: './dua-spinner.component.html',
  providers: [DomHandler, SPINNER_VALUE_ACCESSOR]
})

export class DuaSpinnerComponent extends Spinner implements ControlValueAccessor {

    @Output() onChange: EventEmitter<any> = new EventEmitter();

    @Output() onBlur: EventEmitter<any> = new EventEmitter();

    @Input() step = 1;

    @Input() min: number;

    @Input() max: number;

    @Input() maxlength: number;

    @Input() size: number;

    @Input() placeholder: string;

    @Input() inputId: string;

    @Input() readonly: boolean;

    @Input() decimalSeparator = '.';

    @Input() thousandSeparator = ',';

    @Input() tabindex: number;

    @Input() formatInput = true;

    @Input() type = 'text';

    @Input() required: boolean;

    @Input() compareStyle = false;

    @Input() formControlName: string;

    @Input() parentFormGroup: string;


    constructor(el: ElementRef, domHandler: DomHandler) {
        super(el, domHandler);
    }

    onBlured(event) {
      this.onBlur.emit(event);
    }

    onChanged(event) {
      this.onChange.emit(event);
    }
}
