import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    forwardRef,
    Injector,
    ViewEncapsulation
} from '@angular/core';
import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { CsOptionComponent } from '../option';
import { OptionsFieldBase } from '../core/options-field-base';


// Create an provider object
export const CHECKBOX_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CsCheckboxComponent),
  multi: true
};


@Component({
    selector: 'cs-checkbox',
    templateUrl: './checkbox.component.html',
    styleUrls: ['../forms.css', './checkbox.component.css'],
    providers: [CHECKBOX_CONTROL_VALUE_ACCESSOR],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CsCheckboxComponent extends OptionsFieldBase {

    /** Gets or sets wheiher multiple options can be selected  */
    protected get isMultiple(): boolean{ return true; }

    /** Creates a new instance of the CsCheckboxComponent class */
    constructor(
        _changeDetectorRef: ChangeDetectorRef,
        injector: Injector ) {
            super( _changeDetectorRef, injector );
    }
}
