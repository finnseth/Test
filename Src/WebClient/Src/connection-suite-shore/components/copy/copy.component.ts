import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-copy',
  templateUrl: './copy.component.html',
  styleUrls: ['./copy.component.scss']
})
export class CopyComponent implements OnInit {

  display = false;
  selectedFields: CopyField[];

  @Input() fieldsToCopy: CopyField[];
  @Input() fields: any[];
  @Output() onCopySetting = new EventEmitter<CopyField[]>();

  constructor() { }

  ngOnInit() {
  }

  showDialog() {
    this.display = true;
    this.selectedFields = [];
    if (this.fields !== undefined) {
      for (const key in this.fields) {
        for (const copyField of this.fieldsToCopy) {
          if (copyField.key === key) {
            if (copyField.value !== this.fields[key]) {
              this.selectedFields.push(copyField);
            }
          }
        }
      }
    } else {
      this.selectedFields = this.fieldsToCopy;
    }
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
