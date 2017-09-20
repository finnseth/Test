import { Observable } from 'rxjs/Rx';
import { observable } from 'rxjs/symbol/observable';
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
      this._obs = this.parentFormGroup.controls[this.formControlName].valueChanges.subscribe( changes => {
        if (this._isEnabled) {
          this.checkChanges(changes);
        }
      });
      this.checkChanges(this.parentFormGroup.value[this.formControlName]);
    } else {
      this.el.nativeElement.classList.remove('input-compare');
      if (this._obs) this._obs.unsubscribe();
    }
  };

  _compareForm: FormGroup;
  _isEnabled: boolean;
  _obs: any;

  constructor(private el: ElementRef) {}

  ngOnInit() {
  }

  private checkChanges(changes): void {
    
    let compareValue = undefined;
    if (this._compareForm !== undefined) {
      if (this._compareForm.value.hasOwnProperty(this.formControlName)) {
        compareValue = this._compareForm.value[this.formControlName];
      }
    }

    if ( changes !== compareValue ) {
      this.el.nativeElement.classList.add('input-compare');
    } else {
      this.el.nativeElement.classList.remove('input-compare');
    }
  }
}
