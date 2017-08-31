import {
    ChangeDetectionStrategy,
    ChangeDetectorRef,
    Component,
    ElementRef,
    Injector,
    Input,
    ViewChild,
    ViewEncapsulation,
    forwardRef,
} from '@angular/core';

import { NG_VALUE_ACCESSOR } from '@angular/forms';
import { SingleFieldBase } from 'connection-suite/forms/core/single-field-base';

// Create an provider object
export const INPUT_CONTROL_VALUE_ACCESSOR: any = {
  provide: NG_VALUE_ACCESSOR,
  useExisting: forwardRef(() => CsInputComponent),
  multi: true
};

@Component({
    // moduleId: module.id,
    selector: 'cs-input',
    templateUrl: 'input.component.html',
    styleUrls: [ 'input.component.css', '../forms.css' ],
    providers: [INPUT_CONTROL_VALUE_ACCESSOR],
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class CsInputComponent extends SingleFieldBase {

    private _type: string;

    /** Gets or sets the type of the input */
    @Input()
    get type(){ return this._type; }
    set type( value: string ) {
        this._type = value || 'text';
    }

    /** Creates a new instance of the CsInputComponent class */
    constructor(_changeDetectorRef: ChangeDetectorRef, injector: Injector   ) {

        super( _changeDetectorRef, injector);
    }

    protected AfterContentInit(): void {
        super.AfterContentInit();

        if ( this.fc['__type'] !== undefined ) {
            this.type = this.fc['__type'];
        } else {
            this.type = 'text';
        }
    }


    /** Ivoked when the input control value is changed. */
    _onInteractionEvent( event: Event ) {

        event.stopPropagation();
        super.onValueChanged(event.currentTarget['value']);
    }
}
