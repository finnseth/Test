import { OptionsFieldBase } from '../core/options-field-base';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { ChangeDetectorRef, Component, forwardRef, Injector, ViewEncapsulation } from '@angular/core';

export const SELECT_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CsSelectComponent),
  multi: true
};

@Component({
    selector: 'cs-select',
    templateUrl: 'select.component.html',
    styleUrls: ['select.component.css', '../forms.css'],
    encapsulation: ViewEncapsulation.None,
    providers: [SELECT_VALUE_ACCESSOR]

})
export class CsSelectComponent extends OptionsFieldBase {

    /** Gets or sets wheiher multiple options can be selected  */
    protected get isMultiple(): boolean{ return false; }

    /** Creates a new instance of the CsSelectComponent class */
    constructor(
        _changeDetectorRef: ChangeDetectorRef,
        injector: Injector ) {
            super( _changeDetectorRef, injector );
    }
}
