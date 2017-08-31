import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-applyrow',
  templateUrl: './applyRow.component.html',
  styleUrls: ['./applyRow.component.css']
})
export class ApplyRowComponent implements OnInit {

  @Output() onApply = new EventEmitter<any>();
  @Output() onCancel = new EventEmitter<any>();
  @Input() shipStyle = true;

  constructor() { }

  ngOnInit() {
  }

  onCancelClicked(event: Event) {
    this.onCancel.emit(event);
  }

  onApplyClicked(event: Event) {
    this.onApply.emit(event);
  }
}
