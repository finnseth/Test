import { comparefield } from '../../../modules/configuration/dualog.controller';
import { FormGroup } from '@angular/forms';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'dua-copy',
  templateUrl: './copy.component.html',
  styleUrls: ['./copy.component.scss']
})
export class CopyComponent implements OnInit {

  display = false;
  selectedFields: CopyField[];
  copyField: CopyField[] = [];


  @Input() fieldsToCopy: comparefield[];
  @Input() copyfromform: FormGroup;
  @Input() copytoform: FormGroup;


  @Output() onCopySetting = new EventEmitter<CopyField[]>();

  constructor() { }

  ngOnInit() {
debugger;
    for (const field of this.fieldsToCopy ){
      let value = "";
      if (this.copyfromform.value.hasOwnProperty(field.key)) value = this.copyfromform.value[field.key];
      this.copyField.push({name: field.prettyname, value: value})
    }
  }

  showDialog() {
    this.display = true;
    this.selectedFields = [];
    if (this.fieldsToCopy !== undefined) {
      for (const field of this.fieldsToCopy) {
        let valuefrom = "";
        if (this.copyfromform.value.hasOwnProperty(field.key)) valuefrom = this.copyfromform.value[field.key];
        let valueto = "";
        if (this.copytoform.value.hasOwnProperty(field.key)) valueto = this.copytoform.value[field.key];

        if (valuefrom !== valueto){
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

export interface CopyField {
  name: string;
  value: any;
}
