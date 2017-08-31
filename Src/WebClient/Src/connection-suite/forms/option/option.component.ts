import { Component, Directive, ElementRef, EventEmitter, Input, Output, ViewEncapsulation } from '@angular/core';


export class CsOptionEvent {
    constructor(public source: CsOptionComponent, public isUserInput = false) {}
}

@Component({
    selector: 'cs-option',
    template: `<option value="value" selected="selected">{{text}}</option>`,
    encapsulation: ViewEncapsulation.None
})
export class CsOptionComponent {
    public _text: string;
    public _value: any;
    private _selected = false;

    @Input()
    /** Gets or sets the text of the option. */
    get text(){ return this._text; }
    set text( value: string){ this._text = value; }

    @Input()
    /** Gets or sets the value of the option. */
    get value() {
        return this._value ? this._value : this.text;
    }
    set value( value: any ) { this._value = value; }


    @Input()
    /** Gets or sets the selected state of the option */
    get selected() { return this._selected; }
    set selected(value: boolean) {

        if (this._selected !== value) {
            this._selected = value;
            this.onSelectedChange.emit( new CsOptionEvent(this, false));
        }
    }

    @Output() onSelectedChange  = new EventEmitter<CsOptionEvent>();

    constructor( private _elementRef: ElementRef ) { }
}
