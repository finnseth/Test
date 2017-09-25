import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Component({
  selector: 'dua-copy',
  templateUrl: './copy.component.html',
  styleUrls: ['./copy.component.scss']
})
export class CopyComponent implements OnInit {

  display = false;
  selectedFields: CopyField[];
  copyField: CopyField[] = [];

  @Input() fieldsToCopy: CompareField[];
  @Input() copyfromform: FormGroup;
  @Input() copytoform: FormGroup;
  @Input() layout = 'button';


  @Output() onCopySetting = new EventEmitter<CopyField[]>();

  constructor() { }

  ngOnInit() {
    for (const field of this.fieldsToCopy ){
      let value = '';
      if (this.copyfromform.value.hasOwnProperty(field.key)) {
        value = this.copyfromform.value[field.key];
      }
      this.copyField.push({ name: field.prettyname, value: value, key: field.key })
    }
  }

  showDialog() {
    this.display = true;
    this.selectedFields = [];
    if (this.fieldsToCopy !== undefined) {
      for (const field of this.fieldsToCopy) {

        const valuefrom = (this.copyfromform.value.hasOwnProperty(field.key)) ? this.copyfromform.value[field.key] : '';
        const valueto = (this.copytoform.value.hasOwnProperty(field.key)) ? this.copytoform.value[field.key] : '';

        if (valuefrom !== valueto) {
          this.selectedFields.push({name: field.prettyname, value: valuefrom, key: field.key});
        }
      }
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

export interface CompareField {
  key: string,
  prettyname: string
}
