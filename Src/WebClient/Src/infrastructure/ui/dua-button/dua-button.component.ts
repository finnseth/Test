import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-button',
  templateUrl: './dua-button.component.html',
  styleUrls: ['./dua-button.component.scss']
})
export class DuaButtonComponent implements OnInit {

  @Input() label: string;
  @Input() icon: string;
  @Input() shipStyle = true;
  @Input() focus = true;
  @Input() disabled = false;
  @Output() buttonClicked = new EventEmitter<any>();

  constructor() { }

  ngOnInit() {
  }

  onButtonClicked(event: Event) {
    this.buttonClicked.emit(event);
  }
}
