import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { FormGroup } from '@angular/forms';
import { ICompareField } from '../../../shore/ui/configuration/dualog.controller';

@Component({
  selector: 'dua-copy',
  templateUrl: './copy.component.html',
  styleUrls: ['./copy.component.scss']
})
export class CopyComponent implements OnInit {

  display = false;
  selectedFields: ICopyField[];
  copyField: ICopyField[] = [];

  @Input() fieldsToCopy: ICompareField[];
  @Input() copyfromform: FormGroup;
  @Input() copytoform: FormGroup;
  @Input() layout = 'button';


  @Output() onCopySetting = new EventEmitter<ICopyField[]>();

  constructor() { }

  ngOnInit() {
    for (const field of this.fieldsToCopy ){
      let value = '';
      if (this.copyfromform.value.hasOwnProperty(field.key)) {
        value = this.copyfromform.value[field.key];
      }
      this.copyField.push({name: field.prettyname, value: value})
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
          this.selectedFields.push({name: field.prettyname, value: valuefrom});
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

export interface ICopyField {
  name: string;
  value: any;
}
