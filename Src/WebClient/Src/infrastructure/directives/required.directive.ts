import { Directive, ElementRef, Input, OnInit } from '@angular/core';


@Directive({
  selector: '[dua-required]'
})
export class DuaRequiredDirective implements OnInit {

  @Input('dua-required')
  set isRequired(isRequired: boolean) {
    this._isRequired = isRequired;
  };

  _isRequired = false;

  constructor(private el: ElementRef) {
  }

  ngOnInit() {
    if (this._isRequired) {
      this.el.nativeElement.classList.add('input-required');
    } else {
      this.el.nativeElement.classList.remove('input-required');
    }
  }
}
