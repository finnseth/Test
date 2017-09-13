import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-copy',
  templateUrl: './copy.component.html',
  styleUrls: ['./copy.component.scss']
})
export class CopyComponent implements OnInit {

  display = false;
  selectedFields: CopyField[];

  @Input() fields: CopyField[];
  @Output() onCopySetting = new EventEmitter<CopyField[]>();

  constructor() { }

  ngOnInit() {
  }

  showDialog() {
    this.display = true;
    this.selectedFields = this.fields;
  }

  closeDialog() {
    this.display = false;
  }

  copySetting(event: boolean) {
    this.onCopySetting.emit(this.selectedFields);
    this.display = false;
  }
}

export interface CopyField {
  key: string;
  name: string;
  value: any;
}
