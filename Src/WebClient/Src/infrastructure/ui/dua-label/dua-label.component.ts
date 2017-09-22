import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-label',
  templateUrl: './dua-label.component.html',
  styleUrls: ['./dua-label.component.scss']
})
export class DuaLabelComponent implements OnInit {

  @Input() tooltip: string;

  constructor() { }

  ngOnInit() {
  }
}
