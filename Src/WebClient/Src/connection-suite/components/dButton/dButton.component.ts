import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-button',
  templateUrl: './dButton.component.html',
  styleUrls: ['./dButton.component.scss']
})
export class DButtonComponent implements OnInit {

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
