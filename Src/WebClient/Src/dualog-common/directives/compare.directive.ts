import { Directive, ElementRef, Input, OnInit } from '@angular/core';

import { FormGroup } from '@angular/forms';

@Directive({ 
  selector: '[duaCompare]'
})
export class DuaCompareDirective implements OnInit {

  @Input('duaCompare')
  set compareForm(form: FormGroup) {
    this._compareForm = form;
  };

  @Input() parentFormGroup: FormGroup;
  @Input() formControlName: string;
  @Input('isCompareEnabled')
  set isCompareEnabled(isEnabled: boolean) {
    this._isEnabled = isEnabled;
    if ( this._isEnabled ) {
      this.checkChanges(this.parentFormGroup.value);
    } else {
      this.el.nativeElement.classList.remove('input-compare');
    }
  };

  _compareForm: FormGroup;
  _isEnabled: boolean;

  constructor(private el: ElementRef) {}

  ngOnInit() {

    this.parentFormGroup.valueChanges.subscribe( changes => {
      if (this._isEnabled) {
        this.checkChanges(changes);
      }
    });
  }

  private checkChanges(changes): void {
    let compareValue = undefined;
    if (this._compareForm !== undefined) {
      if (this._compareForm.value.hasOwnProperty(this.formControlName)) {
        compareValue = this._compareForm.value[this.formControlName];
      }
    }
    if ( changes[this.formControlName] !== compareValue ) {
      this.el.nativeElement.classList.add('input-compare');
    } else {
      this.el.nativeElement.classList.remove('input-compare');
    }
  }
}
