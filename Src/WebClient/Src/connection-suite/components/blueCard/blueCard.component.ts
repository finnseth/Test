import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

import { CopyField } from './../../../connection-suite-shore/components/copy/copy.component';

@Component({
  selector: 'dua-bluecard',
  templateUrl: './blueCard.component.html',
  styleUrls: ['./blueCard.component.scss']
})
export class BlueCardComponent implements OnInit {

  @Input() isComparing = false;
  @Input() fieldsToCopy: CopyField[];
  @Input() fields: any[];

  @Output() onCopySetting = new EventEmitter<CopyField[]>();

  constructor() {}

  ngOnInit() {
  }

  copySetting(fields: CopyField[]) {
    this.onCopySetting.emit(fields);
  }
}
